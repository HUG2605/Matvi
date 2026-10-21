using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Queries;
using Matvi.Back.Infrastructure.Common.Extensions;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Handlers
{
    public class ListarPagosDeCajaQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
    ) : IRequestHandler<ListarPagosDeCajaQry, DataGrd<ListarPagoCajaDto>>
    {
        public async Task<DataGrd<ListarPagoCajaDto>> Handle(
            ListarPagosDeCajaQry request,
            CancellationToken cancellationToken)
        {
            var filter = request.Filter;

            #region Query base

            using var db = await _factory.CreateDbContextAsync(cancellationToken);
            var query = db.Set<OpPagoEtt>()
                .Where(p => p.IdOpCorteCaja == request.IdOpCorteCaja)
                .Select(p => new ListarPagoCajaDto
                {
                    Id = p.Id,
                    CtSucu_Clave = p.Nav_IdCtSucursal.Clave,
                    Numero = p.Numero,
                    Fecha = p.Fecha,
                    CtProv_Nombre = p.Nav_IdCtProveedor.Nombre,
                    CtMediPago_Nombre = p.Nav_IdCtMedioPago.Nombre,
                    MontoTotal = p.MontoTotal,
                    EsBancario = p.Coll_OpMovimientoBancario_IdOpPago.Any()
                });

            #endregion

            #region Ordenamiento

            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                var sort = filter.SortBy.ToLower() == "folio" ? "Numero" : filter.SortBy.ToLower();
                var descending = filter.SortDesc;
                query = query.ApplyOrdering(sort, descending);
            }

            #endregion

            #region Filtro por texto

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchLower = filter.Search.ToLower();
                query = query.Where(x =>
                    EF.Functions.Like(x.Numero.ToString()!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtProv_Nombre!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtMediPago_Nombre!, $"%{searchLower}%")
                );
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

            return new DataGrd<ListarPagoCajaDto>
            {
                TotalItems = totalItems,
                Items = items
            };

            #endregion
        }
    }
}
