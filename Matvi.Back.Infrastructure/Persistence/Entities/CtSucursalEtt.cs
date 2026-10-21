using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtSucursalEtt : IAuditable
{
    public byte Id { get; set; }
    public byte IdCtEmpresa { get; set; }
    public string Nombre { get; set; } = null!;
    public string Clave { get; set; } = null!;
    public string Direccion { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public CtEmpresaEtt Nav_IdCtEmpresa { get; set; } = null!;
    public ICollection<OpCorteCajaEtt> Coll_OpCorteCaja_IdCtSucursal { get; set; } = new List<OpCorteCajaEtt>();
    public ICollection<OpGastoEtt> Coll_OpGasto_IdCtSucursal { get; set; } = new List<OpGastoEtt>();
    public ICollection<OpCobroEtt> Coll_OpCobro_IdCtSucursal { get; set; } = new List<OpCobroEtt>();
    public ICollection<RlProductoSucursalEtt> Coll_RlProductoSucursal_IdCtSucursal { get; set; } = new List<RlProductoSucursalEtt>();
    public ICollection<OpPagoEtt> Coll_OpPago_IdCtSucursal { get; set; } = new List<OpPagoEtt>();
    public ICollection<OpCompraEtt> Coll_OpCompra_IdCtSucursal { get; set; } = new List<OpCompraEtt>();
    public ICollection<OpLoteEtt> Coll_OpLote_IdCtSucursal { get; set; } = new List<OpLoteEtt>();
    public ICollection<OpMovimientoInventarioEtt> Coll_OpMovimientoInventario_IdCtSucursal { get; set; } = new List<OpMovimientoInventarioEtt>();
    public ICollection<OpTraspasoEtt> Coll_OpTraspaso_IdCtSucursalOrigen { get; set; } = new List<OpTraspasoEtt>();
    public ICollection<OpTraspasoEtt> Coll_OpTraspaso_IdCtSucursalDestino { get; set; } = new List<OpTraspasoEtt>();
    public ICollection<BtConsecutivoEtt> Coll_BtConsecutivo_IdCtSucursal { get; set; } = new List<BtConsecutivoEtt>();
    public ICollection<OpPedidoEtt> Coll_OpPedido_IdCtSucursal { get; set; } = new List<OpPedidoEtt>();
    public ICollection<RlUsuarioSucursalEtt> Coll_RlUsuarioSucursal_IdCtSucursal { get; set; } = new List<RlUsuarioSucursalEtt>();
}