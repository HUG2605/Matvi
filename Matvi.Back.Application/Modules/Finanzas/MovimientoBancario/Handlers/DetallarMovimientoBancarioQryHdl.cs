using System.Threading;
using System.Threading.Tasks;
using Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Dtos;
using Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Handlers
{
    class DetallarMovimientoBancarioQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
    ) : IRequestHandler<DetallarMovimientoBancarioQry, DetallarMovimientoBancarioDto?>
    {
                /// <inheritdoc/>
public async Task<DetallarMovimientoBancarioDto?> Handle(
            DetallarMovimientoBancarioQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var mov = await db.Set<OpMovimientoBancarioEtt>()
                .AsNoTracking()
                .Include(x => x.Nav_IdCtCuentaBancaria)
                .Include(x => x.Nav_IdOpCobro)
                    .ThenInclude(c => c!.Nav_IdCtSucursal)
                .Include(x => x.Nav_IdOpCobro)
                    .ThenInclude(c => c!.Nav_IdCtCliente)
                .Include(x => x.Nav_IdOpCobro)
                    .ThenInclude(c => c!.Nav_IdCtUsuarioRecibe)
                .Include(x => x.Nav_IdOpGasto)
                    .ThenInclude(g => g!.Nav_IdCtSucursal)
                .Include(x => x.Nav_IdOpGasto)
                    .ThenInclude(g => g!.Nav_IdCtUsuario)
                .Include(x => x.Nav_IdOpPago)
                    .ThenInclude(p => p!.Nav_IdCtSucursal)
                .Include(x => x.Nav_IdOpPago)
                    .ThenInclude(p => p!.Nav_IdCtProveedor)
                .Include(x => x.Nav_IdOpPago)
                    .ThenInclude(p => p!.Nav_IdCtUsuarioPaga)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (mov == null) return null;

            var dto = new DetallarMovimientoBancarioDto
            {
                Id = mov.Id,
                Fecha = mov.Fecha,
                Tipo = mov.Tipo,
                Monto = mov.Monto,
                Referencia = mov.Referencia,
                Conciliado = mov.Conciliado,

                IdCtCuentaBancaria = mov.IdCtCuentaBancaria,
                Banco = mov.Nav_IdCtCuentaBancaria.Banco,
                NumeroCuenta = mov.Nav_IdCtCuentaBancaria.NumeroCuenta,
                CuentaNombre = mov.Nav_IdCtCuentaBancaria.Nombre
            };

            if (mov.Nav_IdOpCobro != null)
            {
                var cobro = mov.Nav_IdOpCobro;
                dto.OrigenTipo = "Cobro";
                dto.OrigenId = cobro.Id;
                dto.OrigenNumero = cobro.Numero;
                dto.OrigenSucuClave = cobro.Nav_IdCtSucursal.Clave;
                dto.OrigenConcepto = "Cobro a cliente";
                dto.OrigenTercero = cobro.Nav_IdCtCliente.Nombre;
                dto.OrigenFecha = cobro.Fecha;
                dto.OrigenMonto = cobro.MontoTotal;
                dto.OrigenNotas = cobro.Notas;
                dto.OrigenUsuario = $"{cobro.Nav_IdCtUsuarioRecibe.Nombre} {cobro.Nav_IdCtUsuarioRecibe.PrimerApellido}".Trim();
                dto.SucursalClave = cobro.Nav_IdCtSucursal.Clave;
                dto.SucursalNombre = cobro.Nav_IdCtSucursal.Nombre;
            }
            else if (mov.Nav_IdOpGasto != null)
            {
                var gasto = mov.Nav_IdOpGasto;
                dto.OrigenTipo = "Gasto";
                dto.OrigenId = gasto.Id;
                dto.OrigenNumero = gasto.Numero;
                dto.OrigenSucuClave = gasto.Nav_IdCtSucursal.Clave;
                dto.OrigenConcepto = gasto.Concepto;
                dto.OrigenTercero = gasto.Proveedor;
                dto.OrigenFecha = gasto.FechaPago;
                dto.OrigenMonto = gasto.Importe;
                dto.OrigenNotas = gasto.Notas;
                dto.OrigenUsuario = $"{gasto.Nav_IdCtUsuario.Nombre} {gasto.Nav_IdCtUsuario.PrimerApellido}".Trim();
                dto.SucursalClave = gasto.Nav_IdCtSucursal.Clave;
                dto.SucursalNombre = gasto.Nav_IdCtSucursal.Nombre;
            }
            else if (mov.Nav_IdOpPago != null)
            {
                var pago = mov.Nav_IdOpPago;
                dto.OrigenTipo = "Pago";
                dto.OrigenId = pago.Id;
                dto.OrigenNumero = pago.Numero;
                dto.OrigenSucuClave = pago.Nav_IdCtSucursal.Clave;
                dto.OrigenConcepto = "Pago a proveedor";
                dto.OrigenTercero = pago.Nav_IdCtProveedor.Nombre;
                dto.OrigenFecha = pago.Fecha;
                dto.OrigenMonto = pago.MontoTotal;
                dto.OrigenNotas = pago.Notas;
                dto.OrigenUsuario = $"{pago.Nav_IdCtUsuarioPaga.Nombre} {pago.Nav_IdCtUsuarioPaga.PrimerApellido}".Trim();
                dto.SucursalClave = pago.Nav_IdCtSucursal.Clave;
                dto.SucursalNombre = pago.Nav_IdCtSucursal.Nombre;
            }
            else
            {
                dto.OrigenTipo = "Manual";
                dto.OrigenConcepto = "Movimiento directo";
            }

            return dto;
        }
    }
}
