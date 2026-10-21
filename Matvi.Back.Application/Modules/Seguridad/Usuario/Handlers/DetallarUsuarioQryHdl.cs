using Matvi.Back.Application.Modules.Seguridad.Usuario.Dtos;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Helpers;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Handlers
{
    class DetallarUsuarioQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<DetallarUsuarioQry, DetallarUsuarioDto>
    {
        public async Task<DetallarUsuarioDto> Handle(
            DetallarUsuarioQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtUsuarioEtt>()
                .Where(cu => cu.Id == request.id)
                .Select(cu => new DetallarUsuarioDto {
                    Id = cu.Id,
                    CtRol_Nombre = cu.Nav_IdCtRol!.Nombre,
                    Nombre = cu.Nombre,
                    PrimerApellido = cu.PrimerApellido,
                    SegundoApellido = cu.SegundoApellido,
                    Direccion = cu.Direccion,
                    Telefono = cu.Telefono,
                    Usuario = cu.Usuario,
                    Activo = cu.Activo
                });

            var usuario = await query.SingleAsync(cancellationToken);
            usuario.Sucursales = await UsuarioQueriesHelper
                .GetSucursalesRelacionadas(db, request.id)
                .ToListAsync(cancellationToken);

            return usuario!;
        }
    }
}
