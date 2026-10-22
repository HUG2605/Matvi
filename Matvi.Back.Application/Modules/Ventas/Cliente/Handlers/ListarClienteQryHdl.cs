using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Ventas.Cliente.Dtos;
using Matvi.Back.Application.Modules.Ventas.Cliente.Queries;
using Matvi.Back.Infrastructure.Common.Extensions;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Handlesrs
{
    class ListarClienteQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarClienteQry, DataGrd<ListarClienteDto>>
    {
                /// <inheritdoc/>
public async Task<DataGrd<ListarClienteDto>> Handle(
            ListarClienteQry request, 
            CancellationToken cancellationToken)
        {
            var filter = request.filter;

            #region Query base
            
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtClienteEtt>()
                .Select(cc => new ListarClienteDto {
                    Id = cc.Id,
                    Nombre = cc.Nombre,
                    Alias = cc.Alias,
                    Direccion = cc.Direccion,
                    Telefono = cc.Telefono,
                    LimiteCreditoSemanal = cc.LimiteCreditoSemanal
                });

            #endregion

            #region Filtro por texto

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchLower = filter.Search.ToLower();
                query = query.Where(x =>
                    EF.Functions.Like(x.Nombre!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.Alias!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.Direccion!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.Telefono!, $"%{searchLower}%")
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

            return new DataGrd<ListarClienteDto>
            {
                TotalItems = totalItems,
                Items = items
            };

            #endregion
        }
    }
}
