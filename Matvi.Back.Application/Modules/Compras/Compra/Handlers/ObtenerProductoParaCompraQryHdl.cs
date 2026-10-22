using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Compras.Compra.Dtos;
using Matvi.Back.Application.Modules.Compras.Compra.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Compras.Compra.Handlers
{
    class ObtenerProductoParaCompraQryHdl(
        IDbContextFactory<MatviQryDbCtx> factory
    ) : IRequestHandler<ObtenerProductoParaCompraQry, OperationResult<AgregarProductoCompraDto?>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<AgregarProductoCompraDto?>> Handle(
            ObtenerProductoParaCompraQry request,
            CancellationToken cancellationToken)
        {
            using var db = await factory.CreateDbContextAsync(cancellationToken);

            var dto = await db.Set<CtPresentacionProductoEtt>()
                .Where(i => i.Id == request.idPresentacionProducto)
                .Select(i => new AgregarProductoCompraDto
                {
                    CtProd_Nombre = i.Nav_IdCtProducto.Nombre,
                    CtProd_EspecificacionTecnica = i.Nav_IdCtProducto.EspecificacionTecnica,
                    CtProd_Marca = i.Nav_IdCtProducto.Marca,
                    CtPresProd_IdCtProducto = i.IdCtProducto,
                    CtPresProd_Nombre = i.Nombre,
                    CtPresProd_FactorUBI = i.FactorUBI,
                    CtPresProd_IdFld = i.Id,
                    CtUnidBaseInve_Abreviatura = i.Nav_IdCtProducto
                                                    .Nav_IdCtUnidadBaseInventario.Abreviatura,
                    CantidadFld = request.cantidad,
                    CostoUP = request.costoUP,
                    CostoUBI = i.FactorUBI == 0
                                                   ? 0
                                                   : Math.Round(request.costoUP / i.FactorUBI, 5),
                    Importe = request.cantidad * request.costoUP
                })
                .SingleOrDefaultAsync(cancellationToken);

            if (dto == null)
                return OperationResult<AgregarProductoCompraDto?>.Info(null,
                    "No se encontró la presentación.");

            if (request.idSucursal > 0)
            {
                var rel = await db.Set<RlProductoSucursalEtt>()
                    .FirstOrDefaultAsync(r => r.IdCtProducto == dto.CtPresProd_IdCtProducto
                                           && r.IdCtSucursal == request.idSucursal, cancellationToken);

                if (rel == null || !rel.Regularizado)
                {
                    return OperationResult<AgregarProductoCompraDto?>.Info(null,
                        $"El producto '{dto.CtProd_Nombre}' no está regularizado en el inventario de esta sucursal. Debe regularizarse antes de su primera compra.");
                }
            }

            return OperationResult<AgregarProductoCompraDto?>.Success(dto, "");
        }
    }
}