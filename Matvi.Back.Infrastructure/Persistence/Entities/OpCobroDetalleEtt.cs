using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class OpCobroDetalleEtt : IAuditable
{
    public int Id { get; set; }
    public int IdOpCobro { get; set; }
    public int IdOpPedido { get; set; }
    public decimal MontoAplicado { get; set; }
    public OpCobroEtt Nav_IdOpCobro { get; set; } = null!;
    public OpPedidoEtt Nav_IdOpPedido { get; set; } = null!;
}