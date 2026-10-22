using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using Matvi.Back.Application.Modules.Ventas.Pedido.Queries;
using Matvi.Back.Infrastructure.Common.Extensions;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Handlers
{
    class ListarPedidoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarPedidoQry, DataGrd<ListarPedidoDto>>
    {
                /// <inheritdoc/>
public async Task<DataGrd<ListarPedidoDto>> Handle(
            ListarPedidoQry request,
            CancellationToken cancellationToken)
        {
            var filter = request.filter;

            #region Query base

            using var db = await _factory.CreateDbContextAsync(cancellationToken);
            var puedeVerSensibles = request.puedeVerSensibles;

            var baseQuery = db.Set<OpPedidoEtt>()
                .Where(e => e.IdCtSucursal == request.idSucursal);

            #region Filtro por rol

            if (request.idRol == (byte)RolEnm.ALMACENISTA)
            {
                baseQuery = baseQuery.Where(e => e.IdCtEstatusPedido == (byte)EstatusPedidoEnm.EN_PROCESO
                                              || e.IdCtEstatusPedido == (byte)EstatusPedidoEnm.APARTADO);
            }
            else if (request.idRol == (byte)RolEnm.REPARTIDOR)
            {
                // El repartidor solo ve los pedidos que le fueron asignados y están
                // en tránsito (EN_PROCESO bajo su custodia).
                baseQuery = baseQuery.Where(e =>
                    e.IdCtUsuarioRepartidor == request.idUsuario &&
                    e.IdCtEstatusPedido == (byte)EstatusPedidoEnm.EN_PROCESO);
            }

            #endregion

            var query = baseQuery.Select(i => new ListarPedidoDto
            {
                // IIdDto
                Id = i.Id,
                // IFolioDto
                CtSucu_Clave = i.Nav_IdCtSucursal!.Clave,
                Numero = i.Numero,
                // Otros
                CtClie_Nombre = i.Nav_IdCtCliente != null ? i.Nav_IdCtCliente.Nombre : "",
                CtClie_Telefono = i.Nav_IdCtCliente != null ? i.Nav_IdCtCliente.Telefono : "",
                Fecha = i.Coll_OpPedidoEstatus_IdOpPedido
                    .Where(e => e.IdCtEstatusPedido == (byte)EstatusPedidoEnm.CREADO)
                    .Select(e => e.Fecha)
                    .First(),
                CtCanaCapt_Nombre = i.Nav_IdCtCanalCaptura!.Nombre,
                CtEstaPedi_Nombre = i.Nav_IdCtEstatusPedido!.Nombre,
                EntregaDomicilio = i.EntregaDomicilio,
                Total = puedeVerSensibles ? i.Total : 0
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
                    EF.Functions.Like(x.CtClie_Nombre!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtCanaCapt_Nombre!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtEstaPedi_Nombre, $"%{searchLower}%") ||
                    (searchDate.HasValue && x.Fecha.Date == searchDate.Value)
                );
            }

            #endregion

            #region Ordenamiento

            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                var sortBy = filter.SortBy.ToLower() == "folio" ? "Numero" : filter.SortBy.ToLower();
                var descending = filter.SortDesc;
                query = query.ApplyOrdering(sortBy.ToLowerInvariant(), descending);
            }
            else
            {
                query = query.OrderByDescending(x => x.Fecha).ThenByDescending(x => x.Numero);
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

            return new DataGrd<ListarPedidoDto>
            {
                TotalItems = totalItems,
                Items = items
            };

            #endregion

        }
    }
}