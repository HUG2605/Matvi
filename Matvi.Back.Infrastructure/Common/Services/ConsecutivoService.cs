using Matvi.Back.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Infrastructure.Common.Services
{
    /// <summary>
    /// Implementación de <see cref="IConsecutivoService"/> que gestiona la asignación de números secuenciales únicos mediante bloqueos a nivel de aplicación en SQL Server.
    /// </summary>
    public class ConsecutivoService : IConsecutivoService
    {
        private readonly MatviCmdDbCtx _context;

        // Mapeo TipoConsecutivo -> (tabla destino, columna Numero).
        // Se usa SOLO en la rama "primera vez" para sembrar BtConsecutivo con
        // MAX(Numero) real de la tabla destino, evitando colisiones con datos
        // preexistentes (ej. CRG_Matvi.sql cargó OpLote 1..46 antes de que
        // existiera la fila en BtConsecutivo).
        //
        // Formato: QUOTENAME(tabla) y QUOTENAME(columna) para soportar nombres
        // con caracteres especiales.
        private static readonly Dictionary<byte, (string Tabla, string Columna)> TablasDestino =
            new()
            {
                [(byte)1] = ("OpCompra",        "Numero"), // COMPRA
                [(byte)2] = ("OpLote",          "Numero"), // LOTE
                [(byte)3] = ("OpMovimientoInventario", "Numero"), // MOVIMIENTO DE INVENTARIO
                [(byte)4] = ("OpPedido",        "Numero"), // PEDIDO
                [(byte)5] = ("OpPago",          "Numero"), // PAGO
                [(byte)6] = ("OpCobro",         "Numero"), // COBRO
                [(byte)7] = ("OpGasto",         "Numero"), // GASTO
                [(byte)8] = ("OpCorteCaja",     "Numero")  // CORTE DE CAJA
            };

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ConsecutivoService"/>.
        /// </summary>
        /// <param name="context">Contexto de base de datos para operaciones de comandos.</param>
        public ConsecutivoService(
            MatviCmdDbCtx context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<int> GenerarNumeroAsync(
            byte idSucursal,
            byte idTipoConsecutivo,
            CancellationToken cancellationToken)
        {
            // Se serializa por (IdCtSucursal, IdCtTipoConsecutivo) usando
            // sp_getapplock. Esto evita el race condition del MERGE+HOLDLOCK
            // original: cuando la fila destino no existía, dos transacciones
            // podían insertar UltimoNumero=1 y devolver el mismo número,
            // reventando UK_OpLote (y similares) con "Valor duplicado: 1, 1".
            //
            // Patrón:
            //   1. sp_getapplock Exclusive sobre un Resource estable por clave.
            //      -> Garantiza exclusión mutua entre transacciones que pidan
            //         el mismo Resource, sin importar el isolation level.
            //   2. SELECT ... WITH (UPDLOCK, HOLDLOCK) sobre BtConsecutivo.
            //      -> Bloqueo de fila real (UPDLOCK) sostenido hasta fin de tx.
            //   3. Si la fila no existe, se siembra con MAX(Numero) de la tabla
            //      destino (filtrada por IdCtSucursal) y se hace +1 para el
            //      siguiente. Si tampoco hay datos, se siembra con 0.
            //      -> NUNCA devuelve un Numero que ya exista en la tabla destino.
            //   4. OUTPUT INSERTED.UltimoNumero.
            //   5. sp_releaseapplock para liberar el app lock al final.

            // Defaults seguros por si el TipoConsecutivo no está mapeado.
            string tablaDestino = "OpCompra";
            string columnaNumero = "Numero";
            if (TablasDestino.TryGetValue(idTipoConsecutivo, out var t))
            {
                tablaDestino = t.Tabla;
                columnaNumero = t.Columna;
            }

            var sql = $@"
                DECLARE @lockResult INT;
                DECLARE @recurso NVARCHAR(200) = N'BtConsecutivo:' + CAST(@idSucursal AS NVARCHAR(10)) + N':' + CAST(@idTipoConsecutivo AS NVARCHAR(10));

                EXEC @lockResult = sp_getapplock
                    @Resource = @recurso,
                    @LockMode = N'Exclusive',
                    @LockOwner = N'Transaction',
                    @LockTimeout = 5000;

                IF @lockResult < 0
                BEGIN
                    -- No se pudo obtener el lock: reventar para que el caller reintente.
                    DECLARE @msg NVARCHAR(500) = N'No se pudo obtener lock para BtConsecutivo (' + CAST(@idSucursal AS NVARCHAR(10)) + N',' + CAST(@idTipoConsecutivo AS NVARCHAR(10)) + N'). Resultado: ' + CAST(@lockResult AS NVARCHAR(10));
                    RAISERROR(@msg, 16, 1);
                    RETURN;
                END

                DECLARE @nuevoNumero INT;

                -- Asegurar fila y leer último número bajo UPDLOCK+HOLDLOCK
                SELECT @nuevoNumero = UltimoNumero
                FROM BtConsecutivo WITH (UPDLOCK, HOLDLOCK)
                WHERE IdCtSucursal = @idSucursal
                  AND IdCtTipoConsecutivo = @idTipoConsecutivo;

                IF @@ROWCOUNT = 0
                BEGIN
                    -- Primera vez: sembrar con MAX(Numero) real de la tabla destino
                    -- filtrada por IdCtSucursal. ISNULL a 0 garantiza que la
                    -- primera llamada devuelva 1 sin colisionar con datos
                    -- preexistentes (CRG_Matvi.sql carga OpLote 1..46 sin fila
                    -- previa en BtConsecutivo).
                    -- QUOTENAME simple (una sola vez) produce [OpCompra]/[Numero]
                    -- para evitar inyección y admitir nombres con espacios.
                    DECLARE @sqlMax NVARCHAR(MAX) = N'SELECT @maxOut = ISNULL(MAX(' + QUOTENAME(@columnaNumero) + N'), 0) FROM ' + QUOTENAME(@tablaDestino) + N' WHERE IdCtSucursal = @idSucursal';
                    DECLARE @maxOut INT = 0;
                    EXEC sp_executesql @sqlMax, N'@idSucursal TINYINT, @maxOut INT OUTPUT', @idSucursal = @idSucursal, @maxOut = @maxOut OUTPUT;
                    SET @nuevoNumero = @maxOut + 1;

                    INSERT INTO BtConsecutivo (IdCtSucursal, IdCtTipoConsecutivo, UltimoNumero)
                    VALUES (@idSucursal, @idTipoConsecutivo, @nuevoNumero);
                END
                ELSE
                BEGIN
                    SET @nuevoNumero = @nuevoNumero + 1;
                    UPDATE BtConsecutivo
                    SET UltimoNumero = @nuevoNumero
                    WHERE IdCtSucursal = @idSucursal
                      AND IdCtTipoConsecutivo = @idTipoConsecutivo;
                END

                -- Liberar el app lock explícitamente
                EXEC sp_releaseapplock
                    @Resource = @recurso,
                    @LockOwner = N'Transaction';

                SELECT @nuevoNumero AS NuevoNumero;";

            var parameters = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("@idSucursal", idSucursal),
                new Microsoft.Data.SqlClient.SqlParameter("@idTipoConsecutivo", idTipoConsecutivo),
                new Microsoft.Data.SqlClient.SqlParameter("@tablaDestino", tablaDestino),
                new Microsoft.Data.SqlClient.SqlParameter("@columnaNumero", columnaNumero)
            };

            var resultado = await _context.Database
                .SqlQueryRaw<int>(sql, parameters)
                .AsAsyncEnumerable()
                .FirstAsync(cancellationToken);

            return resultado;
        }
    }
}