using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class OpPedidoDetalleEventoEtt : IAuditable
{
    public int Id { get; set; }
    public int IdOpPedidoDetalle { get; set; }
    public byte IdCtTipoEventoPedidoDetalle { get; set; }
    public decimal Cantidad { get; set; }
    public DateTime Fecha { get; set; }
    public short IdCtUsuario { get; set; }
    public short? IdCtUsuarioRecibe { get; set; }
    public string? Notas { get; set; }
    public OpPedidoDetalleEtt Nav_IdOpPedidoDetalle { get; set; } = null!;
    public CtTipoEventoPedidoDetalleEtt Nav_IdCtTipoEventoPedidoDetalle { get; set; } = null!;
    public CtUsuarioEtt Nav_IdCtUsuario { get; set; } = null!;
    public CtUsuarioEtt? Nav_IdCtUsuarioRecibe { get; set; }
    public ICollection<OpMovimientoInventarioEtt> Coll_OpMovimientoInventario_IdOpPedidoDetalleEvento { get; set; } = new List<OpMovimientoInventarioEtt>();
}