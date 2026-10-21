using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Compras.Proveedor.Dtos;
using Matvi.Back.Application.Modules.Compras.Proveedor.Queries;
using Matvi.Back.Infrastructure.Common.Extensions;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Compras.Proveedor.Handlers
{
    class ListarProveedorGridQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarProveedorGridQry, DataGrd<ListarProveedorDto>>
    {
        public async Task<DataGrd<ListarProveedorDto>> Handle(
            ListarProveedorGridQry request, 
            CancellationToken cancellationToken)
        {
            var filter = request.filter;

            #region Query base

            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtProveedorEtt>()
                .Select(cp => new ListarProveedorDto
                {
                    Id = cp.Id,
                    Nombre = cp.Nombre,
                    Direccion = cp.Direccion,
                    Telefono = cp.Telefono,
                    NombreContacto = cp.NombreContacto,
                    TelefonoContacto = cp.TelefonoContacto
                });

            #endregion

            #region Filtro por texto

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchLower = filter.Search.ToLower();
                query = query.Where(x =>
                    EF.Functions.Like(x.Nombre!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.Direccion!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.Telefono!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.NombreContacto!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.TelefonoContacto!, $"%{searchLower}%")
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

            return new DataGrd<ListarProveedorDto>
            {
                TotalItems = totalItems,
                Items = items
            };

            #endregion
        }
    }
}
