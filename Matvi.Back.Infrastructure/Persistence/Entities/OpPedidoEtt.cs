using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class OpPedidoEtt : IAuditable
{
    public int Id { get; set; }
    public byte IdCtSucursal { get; set; }
    public int Numero { get; set; }
    public int IdCtCliente { get; set; }
    public string? NombreClienteMostrador { get; set; }
    public string? TelefonoClienteMostrador { get; set; }
    public byte IdCtCanalCaptura { get; set; }
    public byte IdCtEstatusPedido { get; set; }
    public bool EntregaDomicilio { get; set; }
    public string? DireccionEntrega { get; set; }
    public DateTime? FechaEntregaEstimada { get; set; }
    public string Notas { get; set; } = null!;
    public short? IdCtUsuarioRepartidor { get; set; }
    public decimal Total { get; set; }
    public decimal TotalPagado { get; set; }
    public CtSucursalEtt Nav_IdCtSucursal { get; set; } = null!;
    public CtClienteEtt Nav_IdCtCliente { get; set; } = null!;
    public CtCanalCapturaEtt Nav_IdCtCanalCaptura { get; set; } = null!;
    public CtEstatusPedidoEtt Nav_IdCtEstatusPedido { get; set; } = null!;
    public CtUsuarioEtt? Nav_IdCtUsuarioRepartidor { get; set; }
    public ICollection<BtMonederoMovimientoEtt> Coll_BtMonederoMovimiento_IdOpPedido { get; set; } = new List<BtMonederoMovimientoEtt>();
    public ICollection<OpCobroDetalleEtt> Coll_OpCobroDetalle_IdOpPedido { get; set; } = new List<OpCobroDetalleEtt>();
    public ICollection<OpPedidoDetalleEtt> Coll_OpPedidoDetalle_IdOpPedido { get; set; } = new List<OpPedidoDetalleEtt>();
    public ICollection<OpPedidoEstatusEtt> Coll_OpPedidoEstatus_IdOpPedido { get; set; } = new List<OpPedidoEstatusEtt>();
}