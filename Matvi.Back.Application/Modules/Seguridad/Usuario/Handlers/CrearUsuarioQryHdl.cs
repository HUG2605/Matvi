using Matvi.Back.Application.Modules.Seguridad.Usuario.Dtos;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Helpers;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Handlers
{
    class CrearUsuarioQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<CrearUsuarioQry, CrearUsuarioDto>
    {
        public async Task<CrearUsuarioDto> Handle(
            CrearUsuarioQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var usuario = new CrearUsuarioDto
            {
                RolIdFld = default,
                NombreFld = string.Empty,
                PrimerApellidoFld = string.Empty,
                SegundoApellidoFld = string.Empty,
                DireccionFld = string.Empty,
                TelefonoFld = string.Empty,
                Sucursales = await UsuarioQueriesHelper
                    .GetSucursalesRelacionadas(db, -1)
                    .ToListAsync(cancellationToken)
            };

            return usuario;
        }
    }
}
