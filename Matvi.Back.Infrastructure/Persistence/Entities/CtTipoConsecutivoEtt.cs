using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtTipoConsecutivoEtt : IAuditable
{
    public byte Id { get; set; }
    public string Nombre { get; set; } = null!;
    public ICollection<BtConsecutivoEtt> Coll_BtConsecutivo_IdCtTipoConsecutivo { get; set; } = new List<BtConsecutivoEtt>();
}