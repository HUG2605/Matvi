using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtTipoMovimientoEtt : IAuditable
{
    public byte Id { get; set; }
    public byte Tipo { get; set; }
    public string Nombre { get; set; } = null!;
    public ICollection<OpMovimientoInventarioEtt> Coll_OpMovimientoInventario_IdCtTipoMovimiento { get; set; } = new List<OpMovimientoInventarioEtt>();
}