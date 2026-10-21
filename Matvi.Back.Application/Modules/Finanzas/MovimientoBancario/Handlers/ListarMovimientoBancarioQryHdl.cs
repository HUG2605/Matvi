using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Dtos;
using Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Queries;
using Matvi.Back.Infrastructure.Common.Extensions;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Handlers
{
    class ListarMovimientoBancarioQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
    ) : IRequestHandler<ListarMovimientoBancarioQry, DataGrd<ListarMovimientoBancarioDto>>
    {
        public async Task<DataGrd<ListarMovimientoBancarioDto>> Handle(
            ListarMovimientoBancarioQry request,
            CancellationToken cancellationToken)
        {
            var filter = request.Filter;
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var queryBase = db.Set<OpMovimientoBancarioEtt>().AsNoTracking();

            // Filtro por Sucursal (a través de las operaciones ligadas si aplica)
            if (request.IdCtSucursal.HasValue && request.IdCtSucursal.Value > 0)
            {
                var sucId = request.IdCtSucursal.Value;
                queryBase = queryBase.Where(x =>
                    (x.Nav_IdOpCobro != null && x.Nav_IdOpCobro.IdCtSucursal == sucId) ||
                    (x.Nav_IdOpGasto != null && x.Nav_IdOpGasto.IdCtSucursal == sucId) ||
                    (x.Nav_IdOpPago != null && x.Nav_IdOpPago.IdCtSucursal == sucId)
                );
            }

            // Filtro por Cuenta Bancaria
            if (request.IdCtCuentaBancaria.HasValue && request.IdCtCuentaBancaria.Value > 0)
            {
                queryBase = queryBase.Where(x => x.IdCtCuentaBancaria == request.IdCtCuentaBancaria.Value);
            }

            // Filtro por Tipo (1: Ingreso, 2: Egreso)
            if (request.Tipo.HasValue && request.Tipo.Value > 0)
            {
                queryBase = queryBase.Where(x => x.Tipo == request.Tipo.Value);
            }

            // Filtro por Conciliado
            if (request.Conciliado.HasValue)
            {
                queryBase = queryBase.Where(x => x.Conciliado == request.Conciliado.Value);
            }

            // Filtro por Rango de Fechas
            if (request.FechaInicio.HasValue)
            {
                queryBase = queryBase.Where(x => x.Fecha >= request.FechaInicio.Value);
            }
            if (request.FechaFin.HasValue)
            {
                var fin = request.FechaFin.Value.Date.AddDays(1).AddTicks(-1);
                queryBase = queryBase.Where(x => x.Fecha <= fin);
            }

            var query = queryBase.Select(x => new ListarMovimientoBancarioDto
            {
                Id = x.Id,
                Fecha = x.Fecha,
                Tipo = x.Tipo,
                Monto = x.Monto,
                Referencia = x.Referencia,
                Conciliado = x.Conciliado,
                IdCtCuentaBancaria = x.IdCtCuentaBancaria,
                Banco = x.Nav_IdCtCuentaBancaria.Banco,
                NumeroCuenta = x.Nav_IdCtCuentaBancaria.NumeroCuenta,
                CuentaNombre = x.Nav_IdCtCuentaBancaria.Nombre,

                OrigenTipo = x.IdOpCobro != null ? "Cobro"
                           : (x.IdOpGasto != null ? "Gasto"
                           : (x.IdOpPago != null ? "Pago" : "Manual")),

                OrigenId = x.IdOpCobro ?? x.IdOpGasto ?? x.IdOpPago,

                OrigenNumero = x.IdOpCobro != null ? (int?)x.Nav_IdOpCobro!.Numero
                             : (x.IdOpGasto != null ? (int?)x.Nav_IdOpGasto!.Numero
                             : (x.IdOpPago != null ? (int?)x.Nav_IdOpPago!.Numero : null)),

                OrigenSucuClave = x.IdOpCobro != null ? x.Nav_IdOpCobro!.Nav_IdCtSucursal.Clave
                                : (x.IdOpGasto != null ? x.Nav_IdOpGasto!.Nav_IdCtSucursal.Clave
                                : (x.IdOpPago != null ? x.Nav_IdOpPago!.Nav_IdCtSucursal.Clave : null)),

                OrigenDescripcion = x.IdOpCobro != null ? x.Nav_IdOpCobro!.Nav_IdCtCliente.Nombre
                                  : (x.IdOpGasto != null ? (x.Nav_IdOpGasto!.Concepto + " (" + x.Nav_IdOpGasto.Proveedor + ")")
                                  : (x.IdOpPago != null ? x.Nav_IdOpPago!.Nav_IdCtProveedor.Nombre : null)),

                SucursalClave = x.IdOpCobro != null ? x.Nav_IdOpCobro!.Nav_IdCtSucursal.Clave
                              : (x.IdOpGasto != null ? x.Nav_IdOpGasto!.Nav_IdCtSucursal.Clave
                              : (x.IdOpPago != null ? x.Nav_IdOpPago!.Nav_IdCtSucursal.Clave : null)),

                SucursalNombre = x.IdOpCobro != null ? x.Nav_IdOpCobro!.Nav_IdCtSucursal.Nombre
                               : (x.IdOpGasto != null ? x.Nav_IdOpGasto!.Nav_IdCtSucursal.Nombre
                               : (x.IdOpPago != null ? x.Nav_IdOpPago!.Nav_IdCtSucursal.Nombre : null))
            });

            // Filtro por texto
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchLower = filter.Search.Trim().ToLower();
                query = query.Where(x =>
                    EF.Functions.Like(x.Banco, $"%{searchLower}%") ||
                    EF.Functions.Like(x.NumeroCuenta, $"%{searchLower}%") ||
                    (x.Referencia != null && EF.Functions.Like(x.Referencia, $"%{searchLower}%")) ||
                    (x.OrigenDescripcion != null && EF.Functions.Like(x.OrigenDescripcion, $"%{searchLower}%"))
                );
            }

            // Ordenamiento
            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                var sort = filter.SortBy;
                query = query.ApplyOrdering(sort, filter.SortDesc);
            }
            else
            {
                query = query.OrderByDescending(x => x.Fecha);
            }

            var totalItems = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync(cancellationToken);

            return new DataGrd<ListarMovimientoBancarioDto>
            {
                TotalItems = totalItems,
                Items = items
            };
        }
    }
}
