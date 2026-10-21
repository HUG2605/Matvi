using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class OpTraspasoEtt : IAuditable
{
    public int Id { get; set; }
    public byte IdCtSucursalOrigen { get; set; }
    public byte IdCtSucursalDestino { get; set; }
    public DateTime Fecha { get; set; }
    public short IdCtUsuario { get; set; }
    public CtSucursalEtt Nav_IdCtSucursalOrigen { get; set; } = null!;
    public CtSucursalEtt Nav_IdCtSucursalDestino { get; set; } = null!;
    public CtUsuarioEtt Nav_IdCtUsuario { get; set; } = null!;
    public ICollection<OpTraspasoDetalleEtt> Coll_OpTraspasoDetalle_IdOpTraspaso { get; set; } = new List<OpTraspasoDetalleEtt>();
}