using Matvi.Back.Application.Modules.Compras.Compra.Dtos;
using Matvi.Back.Application.Modules.Compras.Compra.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Compras.Compra.Handlers
{
    public class DetallarCompraQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
    ) : IRequestHandler<DetallarCompraQry, DetallarCompraDto>
    {
        public async Task<DetallarCompraDto> Handle(
            DetallarCompraQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<OpCompraEtt>()
                .Where(c => c.Id == request.id)
                .Select(c => new DetallarCompraDto
                {
                    // IIdDto
                    Id = c.Id,
                    // IFolioDto
                    CtSucu_Clave = c.Nav_IdCtSucursal.Clave,
                    Numero = c.Numero,
                    // Otros
                    CtSucu_Nombre = c.Nav_IdCtSucursal.Nombre,
                    Fecha = c.Fecha,
                    CtProv_Nombre = c.Nav_IdCtProveedor.Nombre,
                    Total = c.Total,
                    TotalPagado = c.TotalPagado,

                    Productos = c.Coll_OpCompraDetalle_IdOpCompra
                        .Select(d => new DetallarCompraDetalleDto
                        {
                            // IIdDto
                            Id = d.Id,
                            // IProductoBaseDto
                            CtProd_Nombre = d.Nav_Cpp_IdCtProducto.Nombre,
                            CtProd_EspecificacionTecnica = d.Nav_Cpp_IdCtProducto.EspecificacionTecnica,
                            CtProd_Marca = d.Nav_Cpp_IdCtProducto.Marca,
                            // IPresentacionProductoBaseDto
                            CtPresProd_IdCtProducto = d.Cpp_IdCtProducto,
                            CtPresProd_Nombre = d.Cpp_Nombre,
                            CtPresProd_FactorUBI = d.Cpp_FactorUBI,
                            // Otros
                            Cantidad = d.Cantidad,
                            CostoUP = d.CostoUP,
                            CostoUBI = d.CostoUBI,
                            Importe = d.Importe,
                            CtUnidBaseInve_Abreviatura = d.Nav_Cpp_IdCtProducto.Nav_IdCtUnidadBaseInventario.Abreviatura

                        }).ToList(),

                    Lotes = c.Coll_OpCompraDetalle_IdOpCompra
                        .Where(d => d.Nav_IdOpLote != null)
                        .Select(d => new DetallarCompraLoteDto
                        {
                            // IIdDto
                            Id = d.Nav_IdOpLote!.Id,
                            // IFolioDto
                            CtSucu_Clave = d.Nav_IdOpLote.Nav_IdCtSucursal.Clave,
                            Numero = d.Nav_IdOpLote.Numero,
                            // IProductoBaseDto
                            CtProd_Nombre = d.Nav_IdOpLote.Nav_Cpp_IdCtProducto.Nombre,
                            CtProd_EspecificacionTecnica = d.Nav_IdOpLote.Nav_Cpp_IdCtProducto.EspecificacionTecnica,
                            CtProd_Marca = d.Nav_IdOpLote.Nav_Cpp_IdCtProducto.Marca,
                            // IPresentacionProductoBaseDto
                            CtPresProd_IdCtProducto = d.Nav_IdOpLote.Cpp_IdCtProducto,
                            CtPresProd_Nombre = d.Nav_IdOpLote.Cpp_Nombre,
                            CtPresProd_FactorUBI = d.Nav_IdOpLote.Cpp_FactorUBI,
                            // Otros
                            CtTipoLote_Nombre = d.Nav_IdOpLote.Nav_IdCtTipoLote.Nombre,
                            CantidadInicialUBI = d.Nav_IdOpLote.CantidadInicialUBI,
                            CantidadDisponibleUBI = d.Nav_IdOpLote.CantidadDisponibleUBI,
                            CostoUP = d.Nav_IdOpLote.CostoUP,
                            CostoUBI = d.Nav_IdOpLote.CostoUBI,
                            CtUnidBaseInve_Abreviatura = d.Nav_IdOpLote!.Nav_Cpp_IdCtProducto.Nav_IdCtUnidadBaseInventario.Abreviatura

                        }).ToList(),

                    Pagos = c.Coll_OpPagoDetalle_IdOpCompra
                        .Select(pd => new DetallarCompraPagoDto
                        {
                            // IIdDto
                            Id = pd.Nav_IdOpPago.Id,
                            // IFolioDto
                            CtSucu_Clave = pd.Nav_IdOpPago.Nav_IdCtSucursal.Clave,
                            Numero = pd.Nav_IdOpPago.Numero,
                            // Otros
                            Fecha = pd.Nav_IdOpPago.Fecha,
                            MontoTotal = pd.Nav_IdOpPago.MontoTotal,
                            CtMediPago_Nombre = pd.Nav_IdOpPago.Nav_IdCtMedioPago.Nombre,
                            CtUsuaPaga_Nombre = pd.Nav_IdOpPago.Nav_IdCtUsuarioPaga.Nombre,
                            MontoAplicado = pd.MontoAplicado
                        }).ToList()
                });

            var compra = await query.SingleAsync(cancellationToken);

            var saldoAcumulado = compra.Total;

            foreach (var pago in compra.Pagos.OrderBy(p => p.Fecha))
            {
                pago.SaldoInicial = saldoAcumulado;
                saldoAcumulado -= pago.MontoAplicado;
            }
            return compra;
        }
    }
}