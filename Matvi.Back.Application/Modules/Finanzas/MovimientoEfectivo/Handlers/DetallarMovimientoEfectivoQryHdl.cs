using System.Threading;
using System.Threading.Tasks;
using Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Dtos;
using Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Handlers
{
    class DetallarMovimientoEfectivoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
    ) : IRequestHandler<DetallarMovimientoEfectivoQry, DetallarMovimientoEfectivoDto?>
    {
                /// <inheritdoc/>
public async Task<DetallarMovimientoEfectivoDto?> Handle(
            DetallarMovimientoEfectivoQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            switch (request.TipoOrigen)
            {
                case "Cobro":
                {
                    var c = await db.Set<OpCobroEtt>().AsNoTracking()
                        .Include(x => x.Nav_IdCtSucursal)
                        .Include(x => x.Nav_IdCtCliente)
                        .Include(x => x.Nav_IdCtUsuarioRecibe)
                        .Include(x => x.Nav_IdOpCorteCaja)
                            .ThenInclude(cc => cc!.Nav_IdCtSucursal)
                        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                    if (c == null) return null;

                    return new DetallarMovimientoEfectivoDto
                    {
                        Id = c.Id,
                        Fecha = c.Fecha,
                        Tipo = 1,
                        OrigenTipo = "Cobro",
                        OrigenId = c.Id,
                        OrigenNumero = c.Numero,
                        OrigenSucuClave = c.Nav_IdCtSucursal.Clave,
                        OrigenConcepto = "Cobro a cliente en caja",
                        OrigenTercero = c.Nav_IdCtCliente.Nombre,
                        Monto = c.MontoTotal,
                        Usuario = $"{c.Nav_IdCtUsuarioRecibe.Nombre} {c.Nav_IdCtUsuarioRecibe.PrimerApellido}".Trim(),
                        SucursalClave = c.Nav_IdCtSucursal.Clave,
                        SucursalNombre = c.Nav_IdCtSucursal.Nombre,
                        Notas = c.Notas,

                        IdOpCorteCaja = c.IdOpCorteCaja,
                        CorteCajaFolio = c.Nav_IdOpCorteCaja != null ? $"{c.Nav_IdOpCorteCaja.Nav_IdCtSucursal.Clave}-CRT{c.Nav_IdOpCorteCaja.Numero.ToString().PadLeft(5, '0')}" : null,
                        CorteCajaFechaApertura = c.Nav_IdOpCorteCaja?.FechaApertura,
                        CorteCajaFechaCierre = c.Nav_IdOpCorteCaja?.FechaCierre,
                        CorteCajaEstado = c.Nav_IdOpCorteCaja != null ? (c.Nav_IdOpCorteCaja.FechaCierre.HasValue ? "Cerrado" : "Abierto") : null
                    };
                }


                case "Gasto":
                {
                    var g = await db.Set<OpGastoEtt>().AsNoTracking()
                        .Include(x => x.Nav_IdCtSucursal)
                        .Include(x => x.Nav_IdCtTipoGasto)
                        .Include(x => x.Nav_IdCtUsuario)
                        .Include(x => x.Nav_IdOpCorteCaja)
                            .ThenInclude(cc => cc!.Nav_IdCtSucursal)
                        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                    if (g == null) return null;

                    return new DetallarMovimientoEfectivoDto
                    {
                        Id = g.Id,
                        Fecha = g.FechaPago,
                        Tipo = 2,
                        OrigenTipo = "Gasto",
                        OrigenId = g.Id,
                        OrigenNumero = g.Numero,
                        OrigenSucuClave = g.Nav_IdCtSucursal.Clave,
                        OrigenConcepto = g.Concepto,
                        OrigenTercero = g.Proveedor,
                        Monto = g.Importe,
                        Usuario = $"{g.Nav_IdCtUsuario.Nombre} {g.Nav_IdCtUsuario.PrimerApellido}".Trim(),
                        SucursalClave = g.Nav_IdCtSucursal.Clave,
                        SucursalNombre = g.Nav_IdCtSucursal.Nombre,
                        Notas = g.Notas,

                        IdOpCorteCaja = g.IdOpCorteCaja,
                        CorteCajaFolio = g.Nav_IdOpCorteCaja != null ? $"{g.Nav_IdOpCorteCaja.Nav_IdCtSucursal.Clave}-CRT{g.Nav_IdOpCorteCaja.Numero.ToString().PadLeft(5, '0')}" : null,
                        CorteCajaFechaApertura = g.Nav_IdOpCorteCaja?.FechaApertura,
                        CorteCajaFechaCierre = g.Nav_IdOpCorteCaja?.FechaCierre,
                        CorteCajaEstado = g.Nav_IdOpCorteCaja != null ? (g.Nav_IdOpCorteCaja.FechaCierre.HasValue ? "Cerrado" : "Abierto") : null
                    };
                }

                case "Pago":
                {
                    var p = await db.Set<OpPagoEtt>().AsNoTracking()
                        .Include(x => x.Nav_IdCtSucursal)
                        .Include(x => x.Nav_IdCtProveedor)
                        .Include(x => x.Nav_IdCtUsuarioPaga)
                        .Include(x => x.Nav_IdOpCorteCaja)
                            .ThenInclude(cc => cc!.Nav_IdCtSucursal)
                        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                    if (p == null) return null;

                    return new DetallarMovimientoEfectivoDto
                    {
                        Id = p.Id,
                        Fecha = p.Fecha,
                        Tipo = 2,
                        OrigenTipo = "Pago",
                        OrigenId = p.Id,
                        OrigenNumero = p.Numero,
                        OrigenSucuClave = p.Nav_IdCtSucursal.Clave,
                        OrigenConcepto = "Pago a proveedor en efectivo",
                        OrigenTercero = p.Nav_IdCtProveedor.Nombre,
                        Monto = p.MontoTotal,
                        Usuario = $"{p.Nav_IdCtUsuarioPaga.Nombre} {p.Nav_IdCtUsuarioPaga.PrimerApellido}".Trim(),
                        SucursalClave = p.Nav_IdCtSucursal.Clave,
                        SucursalNombre = p.Nav_IdCtSucursal.Nombre,
                        Notas = p.Notas,

                        IdOpCorteCaja = p.IdOpCorteCaja,
                        CorteCajaFolio = p.Nav_IdOpCorteCaja != null ? $"{p.Nav_IdOpCorteCaja.Nav_IdCtSucursal.Clave}-CRT{p.Nav_IdOpCorteCaja.Numero.ToString().PadLeft(5, '0')}" : null,
                        CorteCajaFechaApertura = p.Nav_IdOpCorteCaja?.FechaApertura,
                        CorteCajaFechaCierre = p.Nav_IdOpCorteCaja?.FechaCierre,
                        CorteCajaEstado = p.Nav_IdOpCorteCaja != null ? (p.Nav_IdOpCorteCaja.FechaCierre.HasValue ? "Cerrado" : "Abierto") : null
                    };
                }

                default:
                    return null;
            }
        }
    }
}
