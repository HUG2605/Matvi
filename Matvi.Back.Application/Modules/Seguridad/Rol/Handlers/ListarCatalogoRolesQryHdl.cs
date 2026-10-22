using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Modules.Seguridad.Rol.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Seguridad.Rol.Handlers
{
    class ListarCatalogoRolesQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarCatalogoRolesQry, List<BaseDto<byte>>>
    {
                /// <inheritdoc/>
public async Task<List<BaseDto<byte>>> Handle(
            ListarCatalogoRolesQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtRolEtt>()
                .Where(cr => cr.Id != (byte)RolEnm.SYSTEM)
                .Select(cr => new BaseDto<byte>
                {
                    Id = cr.Id,
                    Nombre = cr.Nombre
                });

            var res = await query.ToListAsync(cancellationToken);
            return res;
        }
    }
}
