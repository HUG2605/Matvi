using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtPresentacionProductoEtt : IAuditable
{
    public int Id { get; set; }
    public int IdCtProducto { get; set; }
    public string Nombre { get; set; } = null!;
    public decimal FactorUBI { get; set; }
    public bool EsParaCompra { get; set; }
    public bool EsParaVenta { get; set; }
    public decimal PrecioVenta { get; set; }
    public decimal? PrecioMayoreo { get; set; }
    public short? UnidadesMayoreo { get; set; }
    public CtProductoEtt Nav_IdCtProducto { get; set; } = null!;
}