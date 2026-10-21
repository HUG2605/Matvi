using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Modules.Ventas.Cliente.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Handlesrs
{
    class ListarClienteSelectCoincidenciaQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarClienteSelectCoincidenciaQry, List<BaseDto<int>>>
    {
        public async Task<List<BaseDto<int>>> Handle(
            ListarClienteSelectCoincidenciaQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtClienteEtt>()
                .Where(cc => EF.Functions.Like(cc.Nombre, $"%{request.coincidencia}%"))
                .OrderBy(cc => cc.Nombre)
                .Select(cc => new BaseDto<int>
                {
                    Id = cc.Id,
                    Nombre = $"{cc.Nombre}"
                });

            var result = await query.Take(15).ToListAsync();
            return result;
        }
    }
}
