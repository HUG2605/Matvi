using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Productos.Grupo.Dtos;
using Matvi.Back.Application.Modules.Productos.Grupo.Queries;
using Matvi.Back.Infrastructure.Common.Extensions;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Handlers
{
    class ListarGrupoGridQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarGruposQry, DataGrd<ListarGrupoDto>>
    {
                /// <inheritdoc/>
public async Task<DataGrd<ListarGrupoDto>> Handle(
            ListarGruposQry request, 
            CancellationToken cancellationToken)
        {
            #region Query base

            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtGrupoEtt>()
                .Select(cg => new ListarGrupoDto {
                    Id = cg.Id,
                    Nombre = cg.Nombre,
                    Descripcion = cg.Descripcion,
                    CtSecc_Nombre = cg.Nav_IdCtSeccion!.Nombre
                });

            #endregion

            #region Filtro por texto

            if (!string.IsNullOrWhiteSpace(request.filter.Search))
            {
                var searchLower = request.filter.Search.ToLower();
                query = query.Where(x =>
                    EF.Functions.Like(x.Nombre!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.Descripcion!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtSecc_Nombre!, $"%{searchLower}%")
                );
            }

            #endregion

            #region Ordenamiento

            if (!string.IsNullOrWhiteSpace(request.filter.SortBy))
            {
                var sortBy = request.filter.SortBy.ToLower();
                var descending = request.filter.SortDesc;
                query = query.ApplyOrdering(sortBy.ToLowerInvariant(), descending);
            }

            #endregion

            #region Total antes de paginar

            var totalItems = await query.CountAsync(cancellationToken);

            #endregion

            #region Paginación

            var items = await query
                .Skip((request.filter.Page - 1) * request.filter.PageSize)
                .Take(request.filter.PageSize)
                .ToListAsync(cancellationToken);
            #endregion

            #region Resultado

            return new DataGrd<ListarGrupoDto>
            {
                TotalItems = totalItems,
                Items = items
            };

            #endregion
        }
    }
}