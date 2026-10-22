using Matvi.Back.Application.Modules.Seguridad.Login.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Seguridad.Login.Handlers
{
    class VerificarExisteRelUsuarioSucursalQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<VerificarExisteRelUsuarioSucursalQry, bool>
    {
                /// <inheritdoc/>
public async Task<bool> Handle(
            VerificarExisteRelUsuarioSucursalQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<RlUsuarioSucursalEtt>()
                .Where(rus =>
                    rus.IdCtUsuario == request.idUsuario &&
                    rus.IdCtSucursal == request.idSucursal)
                .Select(rus => rus);

            var res = await query.AnyAsync(cancellationToken);
            return res;
        }
    }
}
