using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class OpPagoEtt : IAuditable
{
    public int Id { get; set; }
    public byte IdCtSucursal { get; set; }
    public int Numero { get; set; }
    public DateTime Fecha { get; set; }
    public int IdCtProveedor { get; set; }
    public decimal MontoTotal { get; set; }
    public byte IdCtMedioPago { get; set; }
    public short IdCtUsuarioPaga { get; set; }
    public string? Notas { get; set; }
    public int? IdOpCorteCaja { get; set; }
    public CtSucursalEtt Nav_IdCtSucursal { get; set; } = null!;
    public CtProveedorEtt Nav_IdCtProveedor { get; set; } = null!;
    public CtMedioPagoEtt Nav_IdCtMedioPago { get; set; } = null!;
    public CtUsuarioEtt Nav_IdCtUsuarioPaga { get; set; } = null!;
    public OpCorteCajaEtt? Nav_IdOpCorteCaja { get; set; }
    public ICollection<OpPagoDetalleEtt> Coll_OpPagoDetalle_IdOpPago { get; set; } = new List<OpPagoDetalleEtt>();
    public ICollection<OpMovimientoBancarioEtt> Coll_OpMovimientoBancario_IdOpPago { get; set; } = new List<OpMovimientoBancarioEtt>();
}