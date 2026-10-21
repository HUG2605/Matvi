using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtProveedorEtt : IAuditable
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Direccion { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public string NombreContacto { get; set; } = null!;
    public string TelefonoContacto { get; set; } = null!;
    public ICollection<OpPagoEtt> Coll_OpPago_IdCtProveedor { get; set; } = new List<OpPagoEtt>();
    public ICollection<RlProductoProveedorEtt> Coll_RlProductoProveedor_IdCtProveedor { get; set; } = new List<RlProductoProveedorEtt>();
    public ICollection<OpCompraEtt> Coll_OpCompra_IdCtProveedor { get; set; } = new List<OpCompraEtt>();
}