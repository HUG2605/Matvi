using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Compras.Compra.Dtos;
using Matvi.Back.Application.Modules.Compras.Compra.Queries;
using Matvi.Back.Infrastructure.Common.Extensions;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Matvi.Back.Application.Modules.Compras.Compra.Handlers
{
    class ListarCompraQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
    ) : IRequestHandler<ListarCompraQry, DataGrd<ListarCompraDto>>
    {

                /// <inheritdoc/>
public async Task<DataGrd<ListarCompraDto>> Handle(
            ListarCompraQry request,
            CancellationToken cancellationToken)
        {
            var filter = request.filter;

            #region Query base

            using var db = await _factory.CreateDbContextAsync(cancellationToken);
            var query = db.Set<OpCompraEtt>()
                .Where(i => i.IdCtSucursal == request.idSucursal)
                .Select(i => new ListarCompraDto
                {
                    // IIdDto
                    Id = i.Id,
                    // IFolioDto
                    CtSucu_Clave = i.Nav_IdCtSucursal.Clave,
                    Numero = i.Numero,
                    // Otros
                    CtProv_Nombre = i.Nav_IdCtProveedor.Nombre,
                    Fecha = i.Fecha,
                    Total = i.Total,
                    TotalPagado=i.TotalPagado
                });

            #endregion

            #region Filtro por texto

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                DateTime? searchDate = null;
                if (DateTime.TryParseExact(filter.Search, "dd/MM/yyyy", new CultureInfo("es-MX"), DateTimeStyles.None, out var parsed))
                {
                    searchDate = parsed.Date;
                }
                var searchLower = filter.Search.ToLower();
                query = query.Where(x =>
                    EF.Functions.Like(x.Numero.ToString()!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtProv_Nombre!, $"%{searchLower}%") ||
                    (searchDate.HasValue && x.Fecha.Date == searchDate.Value) 
                );
            }

            #endregion

            #region Ordenamiento

            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                var sort = filter.SortBy.ToLower() == "folio" ? "Numero" : filter.SortBy.ToLower();
                var descending = filter.SortDesc;
                query = query.ApplyOrdering(sort, descending);
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

            return new DataGrd<ListarCompraDto>
            {
                TotalItems = totalItems,
                Items = items
            };

            #endregion
        }
    }
}
