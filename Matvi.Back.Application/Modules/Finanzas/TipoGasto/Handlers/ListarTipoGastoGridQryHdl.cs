using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Dtos;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Queries;
using Matvi.Back.Infrastructure.Common.Extensions;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.TipoGasto.Handlers
{
    class ListarTipoGastoGridQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarTipoGastoGridQry, DataGrd<ListarTipoGastoDto>>
    {
                /// <inheritdoc/>
public async Task<DataGrd<ListarTipoGastoDto>> Handle(
            ListarTipoGastoGridQry request, 
            CancellationToken cancellationToken)
        {
            var filter = request.filter;

            #region Query base
            
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtTipoGastoEtt>()
                .Select(ctg => new ListarTipoGastoDto
                {
                    Id = ctg.Id,
                    Nombre = ctg.Nombre
                });

            #endregion

            #region Filtro por texto

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchLower = filter.Search.ToLower();
                query = query.Where(x =>
                    EF.Functions.Like(x.Nombre!, $"%{searchLower}%")
                );
            }

            #endregion

            #region Ordenamiento

            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                var sortBy = filter.SortBy.ToLower();
                var descending = filter.SortDesc;
                query = query.ApplyOrdering(sortBy.ToLowerInvariant(), descending);
            }

            #endregion

            #region Total antes de paginar

            var totalItems = await query.CountAsync(cancellationToken);

            #endregion

            #region Paginación

            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync(cancellationToken);

            #endregion

            #region Resultado

            return new DataGrd<ListarTipoGastoDto>
            {
                TotalItems = totalItems,
                Items = items
            };

            #endregion

        }
    }
}
