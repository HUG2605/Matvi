using Matvi.Back.Application.Modules.Seguridad.Login.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Seguridad.Login.Handlers
{
    class VerificarEsUsuarioActivoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<VerificarEsUsuarioActivoQry, bool>
    {
                /// <inheritdoc/>
public async Task<bool> Handle(
            VerificarEsUsuarioActivoQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtUsuarioEtt>()
                .Where(cu => cu.Id == request.id)
                .Select(i => i.Activo);

            var res = await query.FirstAsync(cancellationToken);
            return res;
        }
    }
}
