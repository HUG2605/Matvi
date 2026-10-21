using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Modules.Productos.Seccion.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Handlers
{
    class ListarSeccionSelectQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarSeccionSelectQry, List<BaseDto<byte>>>
    {
        public async Task<List<BaseDto<byte>>> Handle(
            ListarSeccionSelectQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtSeccionEtt>()
                .Select(cs => new BaseDto<byte> {
                    Id = cs.Id,
                    Nombre = cs.Nombre
                });

            var res = await query.ToListAsync(cancellationToken);
            return res;
        }
    }

}
