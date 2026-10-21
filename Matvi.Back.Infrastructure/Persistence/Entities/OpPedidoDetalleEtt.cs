using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class OpPedidoDetalleEtt : IAuditable
{
    public int Id { get; set; }
    public int IdOpPedido { get; set; }
    public byte IdCtEstatusPedidoDetalle { get; set; }
    public int Cpp_IdCtProducto { get; set; }
    public string Cpp_Nombre { get; set; } = null!;
    public decimal Cpp_FactorUBI { get; set; }
    public decimal Cantidad { get; set; }
    public decimal CantidadSurtida { get; set; }
    public decimal CantidadEntregada { get; set; }
    public decimal CantidadRechazada { get; set; }
    public decimal CantidadCancelada { get; set; }
    public decimal Precio { get; set; }
    public decimal Importe { get; set; }
    public int? IdOpMovimientoInventario_Salida { get; set; }
    public int? IdOpMovimientoInventario_Reingreso { get; set; }
    public OpPedidoEtt Nav_IdOpPedido { get; set; } = null!;
    public CtEstatusPedidoDetalleEtt Nav_IdCtEstatusPedidoDetalle { get; set; } = null!;
    public CtProductoEtt Nav_Cpp_IdCtProducto { get; set; } = null!;
    public ICollection<OpPedidoDetalleEventoEtt> Coll_OpPedidoDetalleEvento_IdOpPedidoDetalle { get; set; } = new List<OpPedidoDetalleEventoEtt>();
}