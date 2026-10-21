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
    public class ListarCobrosDeCajaQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
    ) : IRequestHandler<ListarCobrosDeCajaQry, DataGrd<ListarCobroCajaDto>>
    {
        public async Task<DataGrd<ListarCobroCajaDto>> Handle(
            ListarCobrosDeCajaQry request,
            CancellationToken cancellationToken)
        {
            var filter = request.Filter;

            #region Query base

            using var db = await _factory.CreateDbContextAsync(cancellationToken);
            var query = db.Set<OpCobroEtt>()
                .Where(c => c.IdOpCorteCaja == request.IdOpCorteCaja)
                .Select(c => new ListarCobroCajaDto
                {
                    Id = c.Id,
                    CtSucu_Clave = c.Nav_IdCtSucursal.Clave,
                    Numero = c.Numero,
                    Fecha = c.Fecha,
                    CtClie_Nombre = c.Nav_IdCtCliente.Nombre,
                    MontoTotal = c.MontoTotal,
                    CtMediPago_Nombre = c.Nav_IdCtMedioPago.Nombre,
                    EsBancario = c.Coll_OpMovimientoBancario_IdOpCobro.Any()
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
                    EF.Functions.Like(x.CtClie_Nombre!, $"%{searchLower}%") ||
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

            return new DataGrd<ListarCobroCajaDto>
            {
                TotalItems = totalItems,
                Items = items
            };

            #endregion
        }
    }
}
