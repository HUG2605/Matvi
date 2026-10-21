using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtEstatusPedidoEtt : IAuditable
{
    public byte Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public ICollection<OpPedidoEtt> Coll_OpPedido_IdCtEstatusPedido { get; set; } = new List<OpPedidoEtt>();
    public ICollection<OpPedidoEstatusEtt> Coll_OpPedidoEstatus_IdCtEstatusPedido { get; set; } = new List<OpPedidoEstatusEtt>();
}