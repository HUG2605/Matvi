using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Modules.Ventas.MedioPago.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Ventas.MedioPago.Handlers
{
    class ListarMedioPagoSelectQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarMedioPagoSelectQry, List<BaseDto<byte>>>
    {
                /// <inheritdoc/>
public async Task<List<BaseDto<byte>>> Handle(
            ListarMedioPagoSelectQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtMedioPagoEtt>()
                .Select(mp => new BaseDto<byte>
                {
                    Id = mp.Id,
                    Nombre = mp.Nombre
                })
                .OrderBy(mp => mp.Id);

            var res = await query.ToListAsync(cancellationToken);
            return res;
        }
    }
}
