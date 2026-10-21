using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using Matvi.Back.Application.Modules.Ventas.Pedido.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Handlers
{
    class ObtenerProductoSucursalQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ObtenerProductoSucursalQry, ObtenerProductoSucursalDto>
    {
        public async Task<ObtenerProductoSucursalDto> Handle(
            ObtenerProductoSucursalQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var result = await db.Set<RlProductoSucursalEtt>()
                .Where(i => i.IdCtSucursal == request.idCtSucursal
                         && i.IdCtProducto == request.idProducto)
                .Select(i => new ObtenerProductoSucursalDto
                {
                    RlProdSucu_Regularizado = i.Regularizado,
                    CtProd_BajoPedido = i.Nav_IdCtProducto.BajoPedido
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result ?? new ObtenerProductoSucursalDto
            {
                RlProdSucu_Regularizado = false,
                CtProd_BajoPedido = false
            };
        }
    }
}
