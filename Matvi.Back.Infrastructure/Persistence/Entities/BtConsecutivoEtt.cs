using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class BtConsecutivoEtt : IAuditable
{
    public byte IdCtSucursal { get; set; }
    public byte IdCtTipoConsecutivo { get; set; }
    public int UltimoNumero { get; set; }
    public CtSucursalEtt Nav_IdCtSucursal { get; set; } = null!;
    public CtTipoConsecutivoEtt Nav_IdCtTipoConsecutivo { get; set; } = null!;
}