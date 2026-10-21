using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class OpCobroEtt : IAuditable
{
    public int Id { get; set; }
    public byte IdCtSucursal { get; set; }
    public int Numero { get; set; }
    public DateTime Fecha { get; set; }
    public int IdCtCliente { get; set; }
    public decimal MontoTotal { get; set; }
    public byte IdCtMedioPago { get; set; }
    public short IdCtUsuarioRecibe { get; set; }
    public string? Notas { get; set; }
    public int? IdOpCorteCaja { get; set; }
    public CtSucursalEtt Nav_IdCtSucursal { get; set; } = null!;
    public CtClienteEtt Nav_IdCtCliente { get; set; } = null!;
    public CtMedioPagoEtt Nav_IdCtMedioPago { get; set; } = null!;
    public CtUsuarioEtt Nav_IdCtUsuarioRecibe { get; set; } = null!;
    public OpCorteCajaEtt? Nav_IdOpCorteCaja { get; set; }
    public ICollection<OpCobroDetalleEtt> Coll_OpCobroDetalle_IdOpCobro { get; set; } = new List<OpCobroDetalleEtt>();
    public ICollection<OpMovimientoBancarioEtt> Coll_OpMovimientoBancario_IdOpCobro { get; set; } = new List<OpMovimientoBancarioEtt>();
}