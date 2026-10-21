using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class OpMovimientoInventarioEtt : IAuditable
{
    public int Id { get; set; }
    public byte IdCtSucursal { get; set; }
    public int Numero { get; set; }
    public int Cpp_IdCtProducto { get; set; }
    public string Cpp_Nombre { get; set; } = null!;
    public decimal Cpp_FactorUBI { get; set; }
    public decimal CantidadUBI { get; set; }
    public DateTime Fecha { get; set; }
    public byte IdCtTipoMovimiento { get; set; }
    public int? IdOpPedidoDetalleEvento { get; set; }
    public int? IdOpCompraDetalle { get; set; }
    public int? IdOpTraspasoDetalle { get; set; }
    public int IdOpLote { get; set; }
    public decimal? CostoUBI { get; set; }
    public decimal? CostoTotal { get; set; }
    public short IdCtUsuario { get; set; }
    public CtSucursalEtt Nav_IdCtSucursal { get; set; } = null!;
    public CtProductoEtt Nav_Cpp_IdCtProducto { get; set; } = null!;
    public CtTipoMovimientoEtt Nav_IdCtTipoMovimiento { get; set; } = null!;
    public OpPedidoDetalleEventoEtt? Nav_IdOpPedidoDetalleEvento { get; set; }
    public OpCompraDetalleEtt? Nav_IdOpCompraDetalle { get; set; }
    public OpTraspasoDetalleEtt? Nav_IdOpTraspasoDetalle { get; set; }
    public OpLoteEtt Nav_IdOpLote { get; set; } = null!;
    public CtUsuarioEtt Nav_IdCtUsuario { get; set; } = null!;
}