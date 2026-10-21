using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Dtos;
using Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Queries;
using Matvi.Back.Infrastructure.Common.Extensions;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Handlers
{
    class ListarMovimientoEfectivoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
    ) : IRequestHandler<ListarMovimientoEfectivoQry, DataGrd<ListarMovimientoEfectivoDto>>
    {
        public async Task<DataGrd<ListarMovimientoEfectivoDto>> Handle(
            ListarMovimientoEfectivoQry request,
            CancellationToken cancellationToken)
        {
            var filter = request.Filter;
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var sucId = request.IdCtSucursal;
            DateTime? fin = request.FechaFin.HasValue ? request.FechaFin.Value.Date.AddDays(1).AddTicks(-1) : null;

            // 1. Cobros en efectivo
            var qCobros = db.Set<OpCobroEtt>().AsNoTracking()
                .Where(c => c.IdCtSucursal == sucId && c.IdCtMedioPago == 1);
            if (request.FechaInicio.HasValue) qCobros = qCobros.Where(c => c.Fecha >= request.FechaInicio.Value);
            if (fin.HasValue) qCobros = qCobros.Where(c => c.Fecha <= fin.Value);
            if (request.EnCorte.HasValue)
                qCobros = request.EnCorte.Value ? qCobros.Where(c => c.IdOpCorteCaja != null) : qCobros.Where(c => c.IdOpCorteCaja == null);

            var selCobros = qCobros.Select(c => new ListarMovimientoEfectivoDto
            {
                Id = c.Id,
                Fecha = c.Fecha,
                Tipo = (byte)1,
                OrigenTipo = "Cobro",
                OrigenNumero = c.Numero,
                OrigenSucuClave = c.Nav_IdCtSucursal.Clave,
                OrigenDescripcion = c.Nav_IdCtCliente.Nombre,
                Monto = c.MontoTotal,
                Cajero = c.Nav_IdCtUsuarioRecibe.Nombre + " " + c.Nav_IdCtUsuarioRecibe.PrimerApellido,
                IdOpCorteCaja = c.IdOpCorteCaja,
                CorteCajaFolio = c.Nav_IdOpCorteCaja != null ? c.Nav_IdOpCorteCaja.Nav_IdCtSucursal.Clave + "-CRT" + c.Nav_IdOpCorteCaja.Numero.ToString() : null
            });

            // 2. Gastos en efectivo
            var qGastos = db.Set<OpGastoEtt>().AsNoTracking()
                .Where(g => g.IdCtSucursal == sucId && g.IdCtMedioPago == 1);
            if (request.FechaInicio.HasValue) qGastos = qGastos.Where(g => g.FechaPago >= request.FechaInicio.Value);
            if (fin.HasValue) qGastos = qGastos.Where(g => g.FechaPago <= fin.Value);
            if (request.EnCorte.HasValue)
                qGastos = request.EnCorte.Value ? qGastos.Where(g => g.IdOpCorteCaja != null) : qGastos.Where(g => g.IdOpCorteCaja == null);

            var selGastos = qGastos.Select(g => new ListarMovimientoEfectivoDto
            {
                Id = g.Id,
                Fecha = g.FechaPago,
                Tipo = (byte)2,
                OrigenTipo = "Gasto",
                OrigenNumero = g.Numero,
                OrigenSucuClave = g.Nav_IdCtSucursal.Clave,
                OrigenDescripcion = g.Concepto + " (" + g.Proveedor + ")",
                Monto = g.Importe,
                Cajero = g.Nav_IdCtUsuario.Nombre + " " + g.Nav_IdCtUsuario.PrimerApellido,
                IdOpCorteCaja = g.IdOpCorteCaja,
                CorteCajaFolio = g.Nav_IdOpCorteCaja != null ? g.Nav_IdOpCorteCaja.Nav_IdCtSucursal.Clave + "-CRT" + g.Nav_IdOpCorteCaja.Numero.ToString() : null
            });

            // 4. Pagos en efectivo
            var qPagos = db.Set<OpPagoEtt>().AsNoTracking()
                .Where(p => p.IdCtSucursal == sucId && p.IdCtMedioPago == 1);
            if (request.FechaInicio.HasValue) qPagos = qPagos.Where(p => p.Fecha >= request.FechaInicio.Value);
            if (fin.HasValue) qPagos = qPagos.Where(p => p.Fecha <= fin.Value);
            if (request.EnCorte.HasValue)
                qPagos = request.EnCorte.Value ? qPagos.Where(p => p.IdOpCorteCaja != null) : qPagos.Where(p => p.IdOpCorteCaja == null);

            var selPagos = qPagos.Select(p => new ListarMovimientoEfectivoDto
            {
                Id = p.Id,
                Fecha = p.Fecha,
                Tipo = (byte)2,
                OrigenTipo = "Pago",
                OrigenNumero = p.Numero,
                OrigenSucuClave = p.Nav_IdCtSucursal.Clave,
                OrigenDescripcion = "Proveedor: " + p.Nav_IdCtProveedor.Nombre,
                Monto = p.MontoTotal,
                Cajero = p.Nav_IdCtUsuarioPaga.Nombre + " " + p.Nav_IdCtUsuarioPaga.PrimerApellido,
                IdOpCorteCaja = p.IdOpCorteCaja,
                CorteCajaFolio = p.Nav_IdOpCorteCaja != null ? p.Nav_IdOpCorteCaja.Nav_IdCtSucursal.Clave + "-CRT" + p.Nav_IdOpCorteCaja.Numero.ToString() : null
            });

            // Filtrar y concatenar según Tipo y OrigenTipo
            IQueryable<ListarMovimientoEfectivoDto>? query = null;

            void AgregarQuery(IQueryable<ListarMovimientoEfectivoDto> q)
            {
                query = query == null ? q : query.Concat(q);
            }

            if (string.IsNullOrWhiteSpace(request.OrigenTipo))
            {
                if (!request.Tipo.HasValue || request.Tipo.Value == 1)
                {
                    AgregarQuery(selCobros);
                }
                if (!request.Tipo.HasValue || request.Tipo.Value == 2)
                {
                    AgregarQuery(selGastos);
                    AgregarQuery(selPagos);
                }
            }
            else
            {
                switch (request.OrigenTipo)
                {
                    case "Cobro":
                        AgregarQuery(selCobros);
                        break;
                    case "Gasto":
                        AgregarQuery(selGastos);
                        break;
                    case "Pago":
                        AgregarQuery(selPagos);
                        break;
                }
            }

            if (query == null)
            {
                return new DataGrd<ListarMovimientoEfectivoDto>
                {
                    TotalItems = 0,
                    Items = new List<ListarMovimientoEfectivoDto>()
                };
            }

            // Filtro por texto
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchLower = filter.Search.Trim().ToLower();
                query = query.Where(x =>
                    EF.Functions.Like(x.OrigenDescripcion, $"%{searchLower}%") ||
                    EF.Functions.Like(x.Cajero, $"%{searchLower}%") ||
                    (x.CorteCajaFolio != null && EF.Functions.Like(x.CorteCajaFolio, $"%{searchLower}%"))
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

            return new DataGrd<ListarMovimientoEfectivoDto>
            {
                TotalItems = totalItems,
                Items = items
            };
        }
    }
}
