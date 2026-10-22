using Matvi.Back.Application.Modules.Seguridad.Usuario.Dtos;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Helpers;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Handlers
{
    class EditarUsuarioQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<EditarUsuarioQry, EditarUsuarioDto>
    {
                /// <inheritdoc/>
public async Task<EditarUsuarioDto> Handle(
            EditarUsuarioQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtUsuarioEtt>()
                .Where(cu => cu.Id == request.id)
                .Select(cu => new EditarUsuarioDto {
                    Id = cu.Id,
                    RolIdFld = cu.IdCtRol,
                    NombreFld = cu.Nombre,
                    PrimerApellidoFld = cu.PrimerApellido,
                    SegundoApellidoFld = cu.SegundoApellido,
                    DireccionFld = cu.Direccion,
                    TelefonoFld = cu.Telefono,
                    ActivoFld = cu.Activo,
                });

            var usuario = await query.SingleAsync(cancellationToken);
            usuario.Sucursales = await UsuarioQueriesHelper
              .GetSucursalesRelacionadas(db, request.id)
              .ToListAsync(cancellationToken);

            return usuario;
        }
    }
}
