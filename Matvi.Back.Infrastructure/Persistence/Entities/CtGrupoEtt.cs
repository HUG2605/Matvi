using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtGrupoEtt : IAuditable
{
    public short Id { get; set; }
    public byte IdCtSeccion { get; set; }
    public string Nombre { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public CtSeccionEtt Nav_IdCtSeccion { get; set; } = null!;
    public ICollection<CtProductoEtt> Coll_CtProducto_IdCtGrupo { get; set; } = new List<CtProductoEtt>();
}