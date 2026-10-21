using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtUnidadBaseInventarioEtt : IAuditable
{
    public byte Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Abreviatura { get; set; } = null!;
    public ICollection<CtProductoEtt> Coll_CtProducto_IdCtUnidadBaseInventario { get; set; } = new List<CtProductoEtt>();
}