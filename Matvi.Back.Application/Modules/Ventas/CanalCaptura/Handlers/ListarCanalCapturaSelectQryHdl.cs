using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Modules.Ventas.CanalCaptura.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Ventas.CanalCaptura.Handlers
{
    class ListarCanalCapturaSelectQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarCanalCapturaSelectQry, List<BaseDto<byte>>>
    {
        public async Task<List<BaseDto<byte>>> Handle(
            ListarCanalCapturaSelectQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtCanalCapturaEtt>()
                .Select(cc => new BaseDto<byte>
                {
                    Id = cc.Id,
                    Nombre = cc.Nombre
                })
                .OrderBy(cc => cc.Id);

            return await query.ToListAsync(cancellationToken);
        }
    }
}
