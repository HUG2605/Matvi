using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtUsuarioWebPushSuscripcionEtt : IAuditable
{
    public int Id { get; set; }
    public short IdCtUsuario { get; set; }
    public string Endpoint { get; set; } = null!;
    public string P256dh { get; set; } = null!;
    public string Auth { get; set; } = null!;
    public DateTime FechaRegistro { get; set; }
    public bool Activa { get; set; }
    public CtUsuarioEtt Nav_IdCtUsuario { get; set; } = null!;
}