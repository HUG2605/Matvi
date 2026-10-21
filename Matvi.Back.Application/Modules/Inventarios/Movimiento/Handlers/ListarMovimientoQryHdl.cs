using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Inventarios.Movimiento.Dtos;
using Matvi.Back.Application.Modules.Inventarios.Movimiento.Queries;
using Matvi.Back.Infrastructure.Common.Extensions;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Inventarios.Movimiento.Handlers
{
    class ListarMovimientoQryHdl(
        IDbContextFactory<MatviQryDbCtx> factory
        ) : IRequestHandler<ListarMovimientoQry, DataGrd<ListarMovimientoDto>>
    {
        public async Task<DataGrd<ListarMovimientoDto>> Handle(
            ListarMovimientoQry request,
            CancellationToken cancellationToken)
        {
            var filter = request.filter;

            #region Query base

            using var db = await factory.CreateDbContextAsync(cancellationToken);
            var query = db.Set<OpMovimientoInventarioEtt>()
                .Where(i => i.IdCtSucursal == request.idSucursal)
                .Select(i => new ListarMovimientoDto
                {
                    Id = i.Id,
                    CtSucu_Clave = i.Nav_IdCtSucursal.Clave,
                    Numero = i.Numero,
                    CtProd_Nombre = i.Nav_Cpp_IdCtProducto.Nombre,
                    CtProd_EspecificacionTecnica = i.Nav_Cpp_IdCtProducto.EspecificacionTecnica,
                    CtProd_Marca = i.Nav_Cpp_IdCtProducto.Marca,
                    CtPresProd_IdCtProducto = i.Cpp_IdCtProducto,
                    CtPresProd_Nombre = i.Cpp_Nombre,
                    CtPresProd_FactorUBI = i.Cpp_FactorUBI,
                    CtTipoMovimiento_Nombre = i.Nav_IdCtTipoMovimiento.Nombre,
                    Cantidad = i.CantidadUBI,
                    Fecha = i.Fecha
                });

            #endregion

            #region Filtro por texto

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchLower = filter.Search.ToLower();
                query = query.Where(x =>
                    EF.Functions.Like(x.Numero.ToString()!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtTipoMovimiento_Nombre!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtProd_Nombre!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtProd_EspecificacionTecnica!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtProd_Marca!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtPresProd_Nombre!, $"%{searchLower}%")
                );
            }

            #endregion

            #region Ordenamiento

            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                var sort = filter.SortBy.ToLower() == "folio" ? "Numero" : filter.SortBy.ToLower();
                query = query.ApplyOrdering(sort, filter.SortDesc);
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

            return new DataGrd<ListarMovimientoDto>
            {
                TotalItems = totalItems,
                Items = items
            };

            #endregion
        }
    }
}
