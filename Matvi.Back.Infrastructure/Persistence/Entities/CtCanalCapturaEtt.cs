using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtCanalCapturaEtt : IAuditable
{
    public byte Id { get; set; }
    public string Nombre { get; set; } = null!;
    public ICollection<OpPedidoEtt> Coll_OpPedido_IdCtCanalCaptura { get; set; } = new List<OpPedidoEtt>();
}