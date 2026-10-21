using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtTipoLoteEtt : IAuditable
{
    public byte Id { get; set; }
    public string Nombre { get; set; } = null!;
    public ICollection<OpLoteEtt> Coll_OpLote_IdCtTipoLote { get; set; } = new List<OpLoteEtt>();
}