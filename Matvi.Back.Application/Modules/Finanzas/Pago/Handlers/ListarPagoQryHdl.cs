using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Compras.Compra.Dtos;
using Matvi.Back.Application.Modules.Finanzas.Pago.Dtos;
using Matvi.Back.Application.Modules.Finanzas.Pago.Queries;
using Matvi.Back.Infrastructure.Common.Extensions;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.Pago.Handlers
{
    class ListarPagoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
    ) : IRequestHandler<ListarPagoQry, DataGrd<ListarPagoDto>>

    {
                /// <inheritdoc/>
public async Task<DataGrd<ListarPagoDto>> Handle(
            ListarPagoQry request, 
            CancellationToken cancellationToken)
        {
            var filter = request.filter;

            #region Query base

            using var db = await _factory.CreateDbContextAsync(cancellationToken);
            var query = db.Set<OpPagoEtt>()
                .Where(i => i.IdCtSucursal == request.idSucucursal)
                .Select(i => new ListarPagoDto
                {
                    // IIdDto
                    Id = i.Id,
                    // IFolioDto
                    CtSucu_Clave = i.Nav_IdCtSucursal.Clave,
                    Numero = i.Numero,
                    // Otros
                    CtProv_Nombre = i.Nav_IdCtProveedor.Nombre,
                    CtMediPago_Nombre = i.Nav_IdCtMedioPago.Nombre,
                    Fecha = i.Fecha,
                    MontoTotal = i.MontoTotal
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

            return new DataGrd<ListarPagoDto>
            {
                TotalItems = totalItems,
                Items = items
            };

            #endregion
        }
    }
}
