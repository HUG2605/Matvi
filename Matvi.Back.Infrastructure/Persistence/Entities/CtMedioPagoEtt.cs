using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtMedioPagoEtt : IAuditable
{
    public byte Id { get; set; }
    public string Nombre { get; set; } = null!;
    public ICollection<OpGastoEtt> Coll_OpGasto_IdCtMedioPago { get; set; } = new List<OpGastoEtt>();
    public ICollection<OpCobroEtt> Coll_OpCobro_IdCtMedioPago { get; set; } = new List<OpCobroEtt>();
    public ICollection<OpPagoEtt> Coll_OpPago_IdCtMedioPago { get; set; } = new List<OpPagoEtt>();
}