using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtTipoGastoEtt : IAuditable
{
    public byte Id { get; set; }
    public string Nombre { get; set; } = null!;
    public ICollection<OpGastoEtt> Coll_OpGasto_IdCtTipoGasto { get; set; } = new List<OpGastoEtt>();
}