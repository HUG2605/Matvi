using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class OpLoteEtt : IAuditable
{
    public int Id { get; set; }
    public byte IdCtSucursal { get; set; }
    public int Numero { get; set; }
    public byte IdCtTipoLote { get; set; }
    public int Cpp_IdCtProducto { get; set; }
    public string Cpp_Nombre { get; set; } = null!;
    public decimal Cpp_FactorUBI { get; set; }
    public decimal CantidadInicialUBI { get; set; }
    public decimal CantidadDisponibleUBI { get; set; }
    public decimal CostoUP { get; set; }
    public decimal CostoUBI { get; set; }
    public DateTime Fecha { get; set; }
    public CtSucursalEtt Nav_IdCtSucursal { get; set; } = null!;
    public CtTipoLoteEtt Nav_IdCtTipoLote { get; set; } = null!;
    public CtProductoEtt Nav_Cpp_IdCtProducto { get; set; } = null!;
    public ICollection<OpCompraDetalleEtt> Coll_OpCompraDetalle_IdOpLote { get; set; } = new List<OpCompraDetalleEtt>();
    public ICollection<OpMovimientoInventarioEtt> Coll_OpMovimientoInventario_IdOpLote { get; set; } = new List<OpMovimientoInventarioEtt>();
    public ICollection<OpTraspasoDetalleEtt> Coll_OpTraspasoDetalle_IdOpLote_Destino { get; set; } = new List<OpTraspasoDetalleEtt>();
}