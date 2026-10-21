using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class OpGastoEtt : IAuditable
{
    public int Id { get; set; }
    public byte IdCtSucursal { get; set; }
    public int Numero { get; set; }
    public DateTime FechaPago { get; set; }
    public byte IdCtTipoGasto { get; set; }
    public string Proveedor { get; set; } = null!;
    public string Concepto { get; set; } = null!;
    public decimal Importe { get; set; }
    public byte IdCtMedioPago { get; set; }
    public short IdCtUsuario { get; set; }
    public int? IdOpCorteCaja { get; set; }
    public string? Notas { get; set; }
    public CtSucursalEtt Nav_IdCtSucursal { get; set; } = null!;
    public CtTipoGastoEtt Nav_IdCtTipoGasto { get; set; } = null!;
    public CtMedioPagoEtt Nav_IdCtMedioPago { get; set; } = null!;
    public CtUsuarioEtt Nav_IdCtUsuario { get; set; } = null!;
    public OpCorteCajaEtt? Nav_IdOpCorteCaja { get; set; }
    public ICollection<OpMovimientoBancarioEtt> Coll_OpMovimientoBancario_IdOpGasto { get; set; } = new List<OpMovimientoBancarioEtt>();
}