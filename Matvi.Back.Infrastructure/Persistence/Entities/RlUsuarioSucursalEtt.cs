using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class RlUsuarioSucursalEtt : IAuditable
{
    public short IdCtUsuario { get; set; }
    public byte IdCtSucursal { get; set; }
    public CtUsuarioEtt Nav_IdCtUsuario { get; set; } = null!;
    public CtSucursalEtt Nav_IdCtSucursal { get; set; } = null!;
}