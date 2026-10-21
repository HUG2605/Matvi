using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class RlRolAccionEtt : IAuditable
{
    public int Id { get; set; }
    public byte IdCtRol { get; set; }
    public short IdCtAccion { get; set; }
    public bool TieneAcceso { get; set; }
    public CtRolEtt Nav_IdCtRol { get; set; } = null!;
    public CtAccionEtt Nav_IdCtAccion { get; set; } = null!;
}