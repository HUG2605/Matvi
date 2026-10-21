using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class OpMovimientoBancarioEtt : IAuditable
{
    public int Id { get; set; }
    public byte IdCtCuentaBancaria { get; set; }
    public DateTime Fecha { get; set; }
    public byte Tipo { get; set; }
    public decimal Monto { get; set; }
    public string? Referencia { get; set; }
    public bool Conciliado { get; set; }
    public int? IdOpCobro { get; set; }
    public int? IdOpGasto { get; set; }
    public int? IdOpPago { get; set; }
    public CtCuentaBancariaEtt Nav_IdCtCuentaBancaria { get; set; } = null!;
    public OpCobroEtt? Nav_IdOpCobro { get; set; }
    public OpGastoEtt? Nav_IdOpGasto { get; set; }
    public OpPagoEtt? Nav_IdOpPago { get; set; }
}