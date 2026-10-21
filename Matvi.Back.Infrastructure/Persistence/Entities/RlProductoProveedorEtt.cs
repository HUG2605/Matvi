using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class RlProductoProveedorEtt : IAuditable
{
    public int IdCtProducto { get; set; }
    public int IdCtProveedor { get; set; }
    public CtProductoEtt Nav_IdCtProducto { get; set; } = null!;
    public CtProveedorEtt Nav_IdCtProveedor { get; set; } = null!;
}