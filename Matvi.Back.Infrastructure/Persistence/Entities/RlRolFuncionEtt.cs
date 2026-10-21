using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class RlRolFuncionEtt : IAuditable
{
    public int Id { get; set; }
    public byte IdCtRol { get; set; }
    public byte IdCtFuncion { get; set; }
    public bool Consultar { get; set; }
    public bool VerSensibles { get; set; }
    public CtRolEtt Nav_IdCtRol { get; set; } = null!;
    public CtFuncionEtt Nav_IdCtFuncion { get; set; } = null!;
}