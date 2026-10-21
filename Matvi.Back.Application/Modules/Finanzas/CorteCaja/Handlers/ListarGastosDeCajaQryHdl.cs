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
    public class ListarGastosDeCajaQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
    ) : IRequestHandler<ListarGastosDeCajaQry, DataGrd<ListarGastoCajaDto>>
    {
        public async Task<DataGrd<ListarGastoCajaDto>> Handle(
            ListarGastosDeCajaQry request,
            CancellationToken cancellationToken)
        {
            var filter = request.Filter;

            #region Query base

            using var db = await _factory.CreateDbContextAsync(cancellationToken);
            var query = db.Set<OpGastoEtt>()
                .Where(g => g.IdOpCorteCaja == request.IdOpCorteCaja)
                .Select(g => new ListarGastoCajaDto
                {
                    Id = g.Id,
                    CtSucu_Clave = g.Nav_IdCtSucursal.Clave,
                    Numero = g.Numero,
                    FechaPago = g.FechaPago,
                    CtTipoGast_Nombre = g.Nav_IdCtTipoGasto.Nombre,
                    Proveedor = g.Proveedor,
                    Concepto = g.Concepto,
                    Importe = g.Importe,
                    CtMedioPago_Nombre = g.Nav_IdCtMedioPago.Nombre,
                    Notas = g.Notas ?? string.Empty,
                    EsBancario = g.Coll_OpMovimientoBancario_IdOpGasto.Any()
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
                    EF.Functions.Like(x.Proveedor!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.Concepto!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtTipoGast_Nombre!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtMedioPago_Nombre!, $"%{searchLower}%")
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

            return new DataGrd<ListarGastoCajaDto>
            {
                TotalItems = totalItems,
                Items = items
            };

            #endregion
        }
    }
}
