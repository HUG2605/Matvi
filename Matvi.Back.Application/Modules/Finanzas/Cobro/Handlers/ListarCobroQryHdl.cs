using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Finanzas.Cobro.Dtos;
using Matvi.Back.Application.Modules.Finanzas.Cobro.Queries;
using Matvi.Back.Application.Modules.Finanzas.Pago.Dtos;
using Matvi.Back.Infrastructure.Common.Extensions;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.Cobro.Handlers
{
    class ListarCobroQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarCobroQry, DataGrd<ListarCobroDto>>
    {
                /// <inheritdoc/>
public async Task<DataGrd<ListarCobroDto>> Handle(
            ListarCobroQry request,
            CancellationToken cancellationToken)
        {
            var filter = request.filter;

            #region Query base

            using var db = await _factory.CreateDbContextAsync(cancellationToken);
            var query = db.Set<OpCobroEtt>()
                .Where(i => i.IdCtSucursal == request.idSucursal)
                .Select(i => new ListarCobroDto
                {
                    // IIdDto
                    Id = i.Id,
                    // IFolioDto
                    CtSucu_Clave = i.Nav_IdCtSucursal.Clave,
                    Numero = i.Numero,
                    // Otros
                    Fecha= i.Fecha,
                    CtClie_Nombre=i.Nav_IdCtCliente.Nombre,
                    MontoTotal=i.MontoTotal,
                    CtMediPago_Nombre=i.Nav_IdCtMedioPago.Nombre,
                    CtUsuaReci_Nombre= i.Nav_IdCtUsuarioRecibe.Nombre + " " +
                         i.Nav_IdCtUsuarioRecibe.PrimerApellido + " " +
                         i.Nav_IdCtUsuarioRecibe.SegundoApellido
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

            return new DataGrd<ListarCobroDto>
            {
                TotalItems = totalItems,
                Items = items
            };

            #endregion
        }
    }
}
