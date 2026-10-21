using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtSeccionEtt : IAuditable
{
    public byte Id { get; set; }
    public string Nombre { get; set; } = null!;
    public ICollection<CtGrupoEtt> Coll_CtGrupo_IdCtSeccion { get; set; } = new List<CtGrupoEtt>();
}