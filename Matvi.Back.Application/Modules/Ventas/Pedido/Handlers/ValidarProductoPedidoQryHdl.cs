using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using Matvi.Back.Application.Modules.Ventas.Pedido.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Handlers
{
    class ValidarProductoPedidoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ValidarProductoPedidoQry, OperationResult<AgregarProductoPedidoDto?>>
    {

        /// <inheritdoc/>
public async Task<OperationResult<AgregarProductoPedidoDto?>> Handle(
        ValidarProductoPedidoQry request,
        CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            // 1. Obtener presentación + producto
            var agreProd = await db.Set<CtPresentacionProductoEtt>()
                .Where(i => i.Id == request.idPresentacionProducto)
                .Select(i => new AgregarProductoPedidoDto
                {
                    // IProductoBaseDto
                    CtProd_Nombre = i.Nav_IdCtProducto.Nombre,
                    CtProd_EspecificacionTecnica = i.Nav_IdCtProducto.EspecificacionTecnica,
                    CtProd_Marca = i.Nav_IdCtProducto.Marca,
                    // IPresentacionProductoDto
                    CtPresProd_IdCtProducto = i.IdCtProducto,
                    CtPresProd_Nombre = i.Nombre,
                    CtPresProd_FactorUBI = i.FactorUBI,
                    // Otros
                    CtPresProd_IdFld = i.Id,
                    CtProd_BajoPedido = i.Nav_IdCtProducto.BajoPedido,
                    CtProd_EntregaMostrador = i.Nav_IdCtProducto.EntregaMostrador,
                    CtUnidBaseInve_Abreviatura = i.Nav_IdCtProducto.Nav_IdCtUnidadBaseInventario.Abreviatura,
                    CantidadFld = request.cantidad,
                    PrecioNormal = i.PrecioVenta,
                    PrecioMayoreo = i.PrecioMayoreo,
                    UnidadesMayoreo = i.UnidadesMayoreo
                })
                .SingleOrDefaultAsync(cancellationToken);

            if (agreProd == null)
            {
                return OperationResult<AgregarProductoPedidoDto?>.Info(null,
                  "No se encontró la presentación.");
            }

            // 1.1. Determinar si aplica precio de mayoreo.
            // Regla: si la presentación tiene PrecioMayoreo y UnidadesMayoreo configurados,
            // y la cantidad solicitada alcanza el umbral, se utiliza el precio de mayoreo.
            bool aplicoMayoreo =
                agreProd.PrecioMayoreo.HasValue
                && agreProd.UnidadesMayoreo.HasValue
                && agreProd.PrecioMayoreo.Value > 0
                && agreProd.UnidadesMayoreo.Value > 0
                && request.cantidad >= agreProd.UnidadesMayoreo.Value;

            agreProd.AplicoMayoreo = aplicoMayoreo;
            agreProd.Precio = aplicoMayoreo ? agreProd.PrecioMayoreo!.Value : agreProd.PrecioNormal;
            agreProd.Importe = request.cantidad * agreProd.Precio;

            // 2. Calcular cantidad solicitada en UBI
            var cantidadSolicitadaUBI = request.cantidad * agreProd.CtPresProd_FactorUBI;

            // 3. Obtener flags de regularización y stock
            var prodSucursal = await db.Set<RlProductoSucursalEtt>()
                .Where(i =>
                    i.IdCtSucursal == request.idSucursal &&
                    i.IdCtProducto == agreProd.CtPresProd_IdCtProducto)
                .Select(i => new {
                    i.Regularizado,
                    i.StockReservadoUBI
                })
                .FirstOrDefaultAsync(cancellationToken);

            // Si no existe relación producto-sucursal, no está regularizado
            var estaRegularizado = prodSucursal?.Regularizado ?? false;
            var stockReservadoUBI = prodSucursal?.StockReservadoUBI ?? 0;

            bool puedeAgregar = true;
            string? mensajeError = null;

            // 4. Lógica de validación según matriz de reglas
            // Solo validar stock si está regularizado Y NO tiene BajoPedido
            if (estaRegularizado && !agreProd.CtProd_BajoPedido)
            {
                // Después de regularización sin BajoPedido: validar stock
                var existenciaTotalUBI = await db.Set<OpLoteEtt>()
                    .Where(i =>
                        i.IdCtSucursal == request.idSucursal &&
                        i.Cpp_IdCtProducto == agreProd.CtPresProd_IdCtProducto &&
                        i.CantidadDisponibleUBI > 0)
                    .SumAsync(i => (decimal?)i.CantidadDisponibleUBI, cancellationToken) ?? 0;

                // Stock real disponible = stock en lotes - reservado
                var stockRealUBI = existenciaTotalUBI - stockReservadoUBI;

                if (stockRealUBI < cantidadSolicitadaUBI)
                {
                    puedeAgregar = false;
                    mensajeError = $"No hay existencias suficientes para '{agreProd.CtProd_Nombre}'. " +
                                 $"Stock disponible: {stockRealUBI:F2}, solicitado: {cantidadSolicitadaUBI:F2}. " +
                                 "Habilite 'BajoPedido' en el producto si desea permitir pedidos sin stock.";
                }
            }
            // Antes de regularización (Regularizado = false): siempre permitir
            // Con BajoPedido = true: siempre permitir aunque no haya stock

            if (!puedeAgregar)
            {
                return OperationResult<AgregarProductoPedidoDto?>.Info(null, mensajeError);
            }

            // 5. Agregar flags de regularización al DTO para uso posterior
            agreProd.RlProdSucu_Regularizado = estaRegularizado;

            return OperationResult<AgregarProductoPedidoDto?>.Success(agreProd, "");
        }
    }
}