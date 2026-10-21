using Matvi.Back.Application.Modules.Inventarios.Movimiento.Dtos;
using Matvi.Back.Application.Modules.Inventarios.Movimiento.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Inventarios.Movimiento.Handlers
{
    public class DetallarMovimientoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<DetallarMovimientoQry, DetalleMovimientoDto>
    {
        public async Task<DetalleMovimientoDto> Handle(
            DetallarMovimientoQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<OpMovimientoInventarioEtt>()
                .Where(x => x.Id == request.id)
                .Select(x => new DetalleMovimientoDto
                {
                    // IIdDto
                    Id = x.Id,
                    // IFolioDto
                    CtSucu_Clave = x.Nav_IdCtSucursal.Clave,
                    CtSucu_Nombre = x.Nav_IdCtSucursal.Nombre,
                    Numero = x.Numero,
                    // IProductoBaseDto
                    CtProd_Nombre = x.Nav_Cpp_IdCtProducto.Nombre,
                    CtProd_EspecificacionTecnica = x.Nav_Cpp_IdCtProducto.EspecificacionTecnica,
                    CtProd_Marca = x.Nav_Cpp_IdCtProducto.Marca,
                    // IPresentacionProductoBaseDto
                    CtPresProd_IdCtProducto = x.Cpp_IdCtProducto,
                    CtPresProd_Nombre = x.Cpp_Nombre,
                    CtPresProd_FactorUBI = x.Cpp_FactorUBI,
                    // Movimiento
                    CtTipoMovimiento_Nombre = x.Nav_IdCtTipoMovimiento.Nombre,
                    CantidadUBI = x.CantidadUBI,
                    Fecha = x.Fecha,
                    // Referencias a otros documentos
                    IdOpPedidoDetalleEvento = x.IdOpPedidoDetalleEvento,
                    IdOpPedidoDetalle = x.Nav_IdOpPedidoDetalleEvento != null ? x.Nav_IdOpPedidoDetalleEvento.IdOpPedidoDetalle : null,
                    IdOpCompraDetalle = x.IdOpCompraDetalle,
                    IdOpTraspasoDetalle = x.IdOpTraspasoDetalle,
                    IdOpLote = x.IdOpLote,
                    // Costos
                    CostoUBI = x.CostoUBI,
                    CostoTotal = x.CostoTotal,
                    // Usuario
                    CtUsuario_Nombre = x.Nav_IdCtUsuario.Nombre + " " + x.Nav_IdCtUsuario.PrimerApellido + " " + x.Nav_IdCtUsuario.SegundoApellido,
                    // Unidad Base de Inventario
                    CtUnidBaseInve_Abreviatura = x.Nav_Cpp_IdCtProducto.Nav_IdCtUnidadBaseInventario.Abreviatura
                });

            var movimiento = await query.SingleAsync(cancellationToken);
            return movimiento;
        }
    }
}