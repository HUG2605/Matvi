using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtTipoEventoPedidoDetalleEtt : IAuditable
{
    public byte Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public ICollection<OpPedidoDetalleEventoEtt> Coll_OpPedidoDetalleEvento_IdCtTipoEventoPedidoDetalle { get; set; } = new List<OpPedidoDetalleEventoEtt>();
}