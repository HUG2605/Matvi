using Matvi.Back.Application.Modules.Productos.Producto.Dtos;
using Matvi.Back.Application.Modules.Productos.Producto.Helpers;
using Matvi.Back.Application.Modules.Productos.Producto.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.Producto.Handlers
{
    class CrearProductoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<CrearProductoQry, CrearProductoDto>
    {
        public async Task<CrearProductoDto> Handle(
            CrearProductoQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var producto = new CrearProductoDto
            {
                CtSecc_IdFld = default,
                CtGrup_IdFld = default,
                NombreFld = string.Empty,
                CtUnidBaseInve_IdFld = default,
                Proveedores = await ProductoQueriesHelper
                    .GetProveedoresRelacionados(db, -1)
                    .ToListAsync(cancellationToken)
            };

            return producto;
        }
    }
}
