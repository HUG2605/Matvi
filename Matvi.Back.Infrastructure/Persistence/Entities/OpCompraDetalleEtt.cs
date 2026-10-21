using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class OpCompraDetalleEtt : IAuditable
{
    public int Id { get; set; }
    public int IdOpCompra { get; set; }
    public int Cpp_IdCtProducto { get; set; }
    public string Cpp_Nombre { get; set; } = null!;
    public decimal Cpp_FactorUBI { get; set; }
    public decimal Cantidad { get; set; }
    public decimal CostoUP { get; set; }
    public decimal CostoUBI { get; set; }
    public decimal Importe { get; set; }
    public int IdOpLote { get; set; }
    public OpCompraEtt Nav_IdOpCompra { get; set; } = null!;
    public CtProductoEtt Nav_Cpp_IdCtProducto { get; set; } = null!;
    public OpLoteEtt Nav_IdOpLote { get; set; } = null!;
    public ICollection<OpMovimientoInventarioEtt> Coll_OpMovimientoInventario_IdOpCompraDetalle { get; set; } = new List<OpMovimientoInventarioEtt>();
}