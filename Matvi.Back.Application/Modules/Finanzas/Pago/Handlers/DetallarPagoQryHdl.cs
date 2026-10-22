using Matvi.Back.Application.Modules.Finanzas.Pago.Dtos;
using Matvi.Back.Application.Modules.Finanzas.Pago.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.Pago.Handlers
{
        /// <summary>
    /// Manejador de consulta encargado de resolver los datos para detallar pago.
    /// </summary>
public class DetallarPagoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
    ) : IRequestHandler<DetallarPagoQry, DetallarPagoDto>
    {
                /// <inheritdoc/>
public async Task<DetallarPagoDto> Handle(
            DetallarPagoQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<OpPagoEtt>()
                .Where(p => p.Id == request.Id)
                .Select(p => new DetallarPagoDto
                {
                    Id = p.Id,
                    CtSucu_Clave = p.Nav_IdCtSucursal.Clave,
                    Numero = p.Numero,
                    CtSucu_Nombre = p.Nav_IdCtSucursal.Nombre,
                    Fecha = p.Fecha,
                    CtProv_Nombre = p.Nav_IdCtProveedor.Nombre,
                    MontoTotal = p.MontoTotal,
                    CtMedioPago_Nombre = p.Nav_IdCtMedioPago.Nombre,
                    CtUsuarioPaga_Nombre = p.Nav_IdCtUsuarioPaga.Nombre + " " +
                         p.Nav_IdCtUsuarioPaga.PrimerApellido + " " +
                         p.Nav_IdCtUsuarioPaga.SegundoApellido,
                    Notas = p.Notas,

                    Compras = p.Coll_OpPagoDetalle_IdOpPago
                        .Select(pd => new DetallarPagoDetalleDto
                        {
                            Id = pd.Nav_IdOpCompra.Id,
                            CtSucu_Clave = pd.Nav_IdOpCompra.Nav_IdCtSucursal.Clave,
                            Numero = pd.Nav_IdOpCompra.Numero,
                            FechaCompra = pd.Nav_IdOpCompra.Fecha,
                            TotalCompra = pd.Nav_IdOpCompra.Total,
                            TotalPagado = pd.Nav_IdOpCompra.TotalPagado,
                            MontoAplicado = pd.MontoAplicado
                        }).ToList()
                });

            var pago = await query.SingleAsync(cancellationToken);
            return pago;
        }
    }
}