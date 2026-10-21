using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtEstatusPedidoDetalleEtt : IAuditable
{
    public byte Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public ICollection<OpPedidoDetalleEtt> Coll_OpPedidoDetalle_IdCtEstatusPedidoDetalle { get; set; } = new List<OpPedidoDetalleEtt>();
}