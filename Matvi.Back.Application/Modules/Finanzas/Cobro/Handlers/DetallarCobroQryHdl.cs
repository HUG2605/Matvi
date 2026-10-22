using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Modules.Finanzas.Cobro.Dtos;
using Matvi.Back.Application.Modules.Finanzas.Cobro.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.Cobro.Handlers
{
        /// <summary>
    /// Manejador de consulta encargado de resolver los datos para detallar cobro.
    /// </summary>
public class DetallarCobroQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
    ) : IRequestHandler<DetallarCobroQry, DetallarCobroDto>
    {
                /// <inheritdoc/>
public async Task<DetallarCobroDto> Handle(
            DetallarCobroQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<OpCobroEtt>()
                .Where(c => c.Id == request.Id)
                .Select(c => new DetallarCobroDto
                {
                    Id = c.Id,
                    CtSucu_Clave = c.Nav_IdCtSucursal.Clave,
                    Numero = c.Numero,
                    CtSucu_Nombre = c.Nav_IdCtSucursal.Nombre,
                    Fecha = c.Fecha,
                    CtClie_Nombre = c.Nav_IdCtCliente.Nombre,
                    MontoTotal = c.MontoTotal,
                    CtMedioPago_Nombre = c.Nav_IdCtMedioPago.Nombre,
                    CtUsuarioRecibe_Nombre = c.Nav_IdCtUsuarioRecibe.Nombre + " " +
                         c.Nav_IdCtUsuarioRecibe.PrimerApellido + " " +
                         c.Nav_IdCtUsuarioRecibe.SegundoApellido,
                    Notas = c.Notas,
                    Pedidos = c.Coll_OpCobroDetalle_IdOpCobro
                        .Select(cd => new DetallarCobroDetalleDto
                        {
                            Id = cd.Nav_IdOpPedido.Id,
                            CtSucu_Clave = cd.Nav_IdOpPedido.Nav_IdCtSucursal.Clave,
                            Numero = cd.Nav_IdOpPedido.Numero,
                            // Fecha del primer estatus "CREADO"
                            FechaPedido = cd.Nav_IdOpPedido.Coll_OpPedidoEstatus_IdOpPedido
                                .Where(e => e.IdCtEstatusPedido == (byte)EstatusPedidoEnm.CREADO)
                                .OrderBy(e => e.Fecha)
                                .Select(e => e.Fecha)
                                .FirstOrDefault(),
                            TotalPedido = cd.Nav_IdOpPedido.Total,
                            TotalPagado = cd.Nav_IdOpPedido.TotalPagado,
                            MontoAplicado = cd.MontoAplicado
                        }).ToList()

                });

            var cobro = await query.SingleAsync(cancellationToken);
            return cobro;
        }
    }
}