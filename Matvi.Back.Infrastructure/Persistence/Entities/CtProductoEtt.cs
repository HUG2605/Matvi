using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtProductoEtt : IAuditable
{
    public int Id { get; set; }
    public short IdCtGrupo { get; set; }
    public string Nombre { get; set; } = null!;
    public string? EspecificacionTecnica { get; set; }
    public string? Marca { get; set; }
    public byte IdCtUnidadBaseInventario { get; set; }
    public string? Imagen { get; set; }
    public string? CodigoBarras { get; set; }
    public bool CodigoBarrasInterno { get; set; }
    public bool BajoPedido { get; set; }
    public bool EntregaMostrador { get; set; }
    public bool Activo { get; set; }
    public CtGrupoEtt Nav_IdCtGrupo { get; set; } = null!;
    public CtUnidadBaseInventarioEtt Nav_IdCtUnidadBaseInventario { get; set; } = null!;
    public ICollection<CtPresentacionProductoEtt> Coll_CtPresentacionProducto_IdCtProducto { get; set; } = new List<CtPresentacionProductoEtt>();
    public ICollection<RlProductoSucursalEtt> Coll_RlProductoSucursal_IdCtProducto { get; set; } = new List<RlProductoSucursalEtt>();
    public ICollection<RlProductoProveedorEtt> Coll_RlProductoProveedor_IdCtProducto { get; set; } = new List<RlProductoProveedorEtt>();
    public ICollection<OpCompraDetalleEtt> Coll_OpCompraDetalle_Cpp_IdCtProducto { get; set; } = new List<OpCompraDetalleEtt>();
    public ICollection<OpLoteEtt> Coll_OpLote_Cpp_IdCtProducto { get; set; } = new List<OpLoteEtt>();
    public ICollection<OpMovimientoInventarioEtt> Coll_OpMovimientoInventario_Cpp_IdCtProducto { get; set; } = new List<OpMovimientoInventarioEtt>();
    public ICollection<OpTraspasoDetalleEtt> Coll_OpTraspasoDetalle_Cpp_IdCtProducto { get; set; } = new List<OpTraspasoDetalleEtt>();
    public ICollection<OpPedidoDetalleEtt> Coll_OpPedidoDetalle_Cpp_IdCtProducto { get; set; } = new List<OpPedidoDetalleEtt>();
}