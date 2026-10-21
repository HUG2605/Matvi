using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtUsuarioEtt : IAuditable
{
    public short Id { get; set; }
    public byte IdCtRol { get; set; }
    public string Nombre { get; set; } = null!;
    public string PrimerApellido { get; set; } = null!;
    public string SegundoApellido { get; set; } = null!;
    public string Direccion { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public string Usuario { get; set; } = null!;
    public string? Contrasenia { get; set; }
    public bool Activo { get; set; }
    public CtRolEtt Nav_IdCtRol { get; set; } = null!;
    public ICollection<OpCorteCajaEtt> Coll_OpCorteCaja_IdCtUsuario { get; set; } = new List<OpCorteCajaEtt>();
    public ICollection<OpGastoEtt> Coll_OpGasto_IdCtUsuario { get; set; } = new List<OpGastoEtt>();
    public ICollection<OpCobroEtt> Coll_OpCobro_IdCtUsuarioRecibe { get; set; } = new List<OpCobroEtt>();
    public ICollection<OpPagoEtt> Coll_OpPago_IdCtUsuarioPaga { get; set; } = new List<OpPagoEtt>();
    public ICollection<OpMovimientoInventarioEtt> Coll_OpMovimientoInventario_IdCtUsuario { get; set; } = new List<OpMovimientoInventarioEtt>();
    public ICollection<OpTraspasoEtt> Coll_OpTraspaso_IdCtUsuario { get; set; } = new List<OpTraspasoEtt>();
    public ICollection<OpPedidoEtt> Coll_OpPedido_IdCtUsuarioRepartidor { get; set; } = new List<OpPedidoEtt>();
    public ICollection<CtUsuarioWebPushSuscripcionEtt> Coll_CtUsuarioWebPushSuscripcion_IdCtUsuario { get; set; } = new List<CtUsuarioWebPushSuscripcionEtt>();
    public ICollection<OpPedidoDetalleEventoEtt> Coll_OpPedidoDetalleEvento_IdCtUsuario { get; set; } = new List<OpPedidoDetalleEventoEtt>();
    public ICollection<OpPedidoDetalleEventoEtt> Coll_OpPedidoDetalleEvento_IdCtUsuarioRecibe { get; set; } = new List<OpPedidoDetalleEventoEtt>();
    public ICollection<RlUsuarioSucursalEtt> Coll_RlUsuarioSucursal_IdCtUsuario { get; set; } = new List<RlUsuarioSucursalEtt>();
    public ICollection<OpPedidoEstatusEtt> Coll_OpPedidoEstatus_IdCtUsuario { get; set; } = new List<OpPedidoEstatusEtt>();
}