using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Enums;
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
    class ListarCorteCajaQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarCorteCajaQry, DataGrd<ListarCorteCajaDto>>
    {
                /// <inheritdoc/>
public async Task<DataGrd<ListarCorteCajaDto>> Handle(
            ListarCorteCajaQry request,
            CancellationToken cancellationToken)
        {
            var filter = request.filter;

            #region Query base

            using var db = await _factory.CreateDbContextAsync(cancellationToken);
            var baseQuery = db.Set<OpCorteCajaEtt>()
                .Where(i => i.IdCtSucursal == request.idSucursal);

            #region Filtro por rol / usuario

            bool esAdmin = request.idRol == (byte)RolEnm.ADMINISTRADOR || request.idRol == (byte)RolEnm.SYSTEM;

            if (!esAdmin)
            {
                // El vendedor sólo ve sus cajas abiertas (FechaCierre == null)
                baseQuery = baseQuery.Where(i => i.IdCtUsuario == request.idUsuario && i.FechaCierre == null);
            }

            #endregion

            var query = baseQuery.Select(i => new ListarCorteCajaDto
                {
                    Id = i.Id,
                    CtSucu_Clave = i.Nav_IdCtSucursal.Clave,
                    Numero = i.Numero,
                    FechaApertura = i.FechaApertura,
                    FechaCierre = i.FechaCierre,
                    CtUsua_Nombre = i.Nav_IdCtUsuario.Nombre + " " +
                        i.Nav_IdCtUsuario.PrimerApellido + " " +
                        i.Nav_IdCtUsuario.SegundoApellido,
                    FondoInicial = i.FondoInicial,
                    Diferencia = i.Diferencia,
                    Activa = i.FechaCierre == null
                });
            #endregion

            #region Ordenamiento

            var sortBy = filter.SortBy?.ToLower();
            var sort = string.IsNullOrWhiteSpace(sortBy)
                ? "FechaApertura"
                : sortBy == "folio" ? "Numero" : sortBy;
            var descending = string.IsNullOrWhiteSpace(filter.SortBy) ? true : filter.SortDesc;

            query = query.ApplyOrdering(sort, descending);

            #endregion

            #region Filtro por texto

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchLower = filter.Search.ToLower();
                query = query.Where(x =>
                    EF.Functions.Like(x.Id.ToString(), $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtUsua_Nombre!, $"%{searchLower}%"));
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

            return new DataGrd<ListarCorteCajaDto>
            {
                TotalItems = totalItems,
                Items = items
            };

            #endregion
        }
    }
}