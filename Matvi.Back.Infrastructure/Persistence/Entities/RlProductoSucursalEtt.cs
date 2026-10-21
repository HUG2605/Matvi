using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class RlProductoSucursalEtt : IAuditable
{
    public int IdCtProducto { get; set; }
    public byte IdCtSucursal { get; set; }
    public decimal StockMinimoUBI { get; set; }
    public decimal StockMaximoUBI { get; set; }
    public bool Regularizado { get; set; }
    public string? Ubicacion { get; set; }
    public decimal StockReservadoUBI { get; set; }
    public CtProductoEtt Nav_IdCtProducto { get; set; } = null!;
    public CtSucursalEtt Nav_IdCtSucursal { get; set; } = null!;
}