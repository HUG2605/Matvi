using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Dtos;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Queries;
using Matvi.Back.Infrastructure.Common.Extensions;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.Gasto.Handlers
{
    class ListarGastoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
    ) : IRequestHandler<ListarGastoQry, DataGrd<ListarGastoDto>>
    {
                /// <inheritdoc/>
public async Task<DataGrd<ListarGastoDto>> Handle(
            ListarGastoQry request,
            CancellationToken cancellationToken)
        {
         
            var filter = request.filter;
            #region Query base
            using var db = await _factory.CreateDbContextAsync(cancellationToken);
            var query = db.Set<OpGastoEtt>()
                .Where(i => i.IdCtSucursal == request.idSucursal)
                .Select(i => new ListarGastoDto
                {
                    // IIdDto
                    Id = i.Id,
                    // IFolioDto
                    CtSucu_Clave = i.Nav_IdCtSucursal.Clave,
                    Numero = i.Numero,
                    // Otros
                    CtTipoGast_Nombre = i.Nav_IdCtTipoGasto.Nombre,
                    Proveedor = i.Proveedor,
                    Importe = i.Importe,
                    CtMedioPago_Nombre = i.Nav_IdCtMedioPago.Nombre,
                    CtUsua_Nombre = i.Nav_IdCtUsuario.Nombre,
                    Notas = i.Notas,
                    FechaPago = i.FechaPago
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
                    EF.Functions.Like(x.Proveedor!, $"%{searchLower}%") 
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

            return new DataGrd<ListarGastoDto>
            {
                TotalItems = totalItems,
                Items = items
            };

            #endregion
        }
    }
}
