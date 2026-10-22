using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using Matvi.Back.Application.Modules.Ventas.Pedido.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Handlers
{
    class ObtenerPresentacionProductoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ObtenerPresentacionProductoQry, ObtenerPresentacionProductoDto>
    {
                /// <inheritdoc/>
public async Task<ObtenerPresentacionProductoDto> Handle(
            ObtenerPresentacionProductoQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            // 1. Datos del producto
            var presProd = await db.Set<CtPresentacionProductoEtt>()
                .Where(i => i.Id == request.idPresentacionProducto)
                .Select(i => new ObtenerPresentacionProductoDto
                {
                    Id = i.Id,
                    FactorUBI = i.FactorUBI,
                    CtProd_Id = i.IdCtProducto,
                    CtProd_BajoPedido = i.Nav_IdCtProducto.BajoPedido,
                    CtProd_EntregaMostrador = i.Nav_IdCtProducto.EntregaMostrador
                })
                .SingleAsync(cancellationToken);

            // 2. Existencias en la presentacion
            presProd.ExistenciasPresentacion = await db.Set<OpLoteEtt>()
                .Where(i =>
                    i.IdCtSucursal == request.idSucursal &&
                    i.Cpp_IdCtProducto == presProd.CtProd_Id &&
                    i.CantidadDisponibleUBI > 0)
                .SumAsync(i => i.CantidadDisponibleUBI, cancellationToken);

              presProd.ExistenciasPresentacion = Math.Round(presProd.ExistenciasPresentacion / presProd.FactorUBI, 2);

            // 3. Regularización
            presProd.RlProdSucu_Regularizado = await db.Set<RlProductoSucursalEtt>()
                .Where(i =>
                    i.IdCtSucursal == request.idSucursal &&
                    i.IdCtProducto == presProd.CtProd_Id)
                .Select(i => i.Regularizado)
                .FirstAsync(cancellationToken);

            return presProd;
        }
    }
}
