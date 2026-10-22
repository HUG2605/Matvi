using Matvi.Back.Application.Modules.Compras.Proveedor.Dtos;
using Matvi.Back.Application.Modules.Compras.Proveedor.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Compras.Proveedor.Handlers
{
    class EditarProveedorQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<EditarProveedorQry, EditarProveedorDto>
    {
                /// <inheritdoc/>
public async Task<EditarProveedorDto> Handle(
            EditarProveedorQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtProveedorEtt>()
                .Where(cp => cp.Id == request.id)
                .Select(cs => new EditarProveedorDto
                {
                    Id = cs.Id,
                    NombreFld = cs.Nombre,
                    DireccionFld = cs.Direccion,
                    TelefonoFld = cs.Telefono,
                    NombreContactoFld = cs.NombreContacto,
                    TelefonoContactoFld = cs.TelefonoContacto
                });
         
            var proveedor = await query.SingleAsync(cancellationToken);
            return proveedor;
        }
    }
}
