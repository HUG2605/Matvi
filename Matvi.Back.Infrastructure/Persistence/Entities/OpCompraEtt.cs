using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class OpCompraEtt : IAuditable
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public byte IdCtSucursal { get; set; }
    public int Numero { get; set; }
    public int IdCtProveedor { get; set; }
    public decimal Total { get; set; }
    public decimal TotalPagado { get; set; }
    public CtSucursalEtt Nav_IdCtSucursal { get; set; } = null!;
    public CtProveedorEtt Nav_IdCtProveedor { get; set; } = null!;
    public ICollection<OpPagoDetalleEtt> Coll_OpPagoDetalle_IdOpCompra { get; set; } = new List<OpPagoDetalleEtt>();
    public ICollection<OpCompraDetalleEtt> Coll_OpCompraDetalle_IdOpCompra { get; set; } = new List<OpCompraDetalleEtt>();
}