using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class OpPagoDetalleEtt : IAuditable
{
    public int Id { get; set; }
    public int IdOpPago { get; set; }
    public int IdOpCompra { get; set; }
    public decimal MontoAplicado { get; set; }
    public OpPagoEtt Nav_IdOpPago { get; set; } = null!;
    public OpCompraEtt Nav_IdOpCompra { get; set; } = null!;
}