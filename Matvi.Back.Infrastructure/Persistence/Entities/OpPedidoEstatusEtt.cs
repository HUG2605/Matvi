using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class OpPedidoEstatusEtt : IAuditable
{
    public int Id { get; set; }
    public int IdOpPedido { get; set; }
    public byte IdCtEstatusPedido { get; set; }
    public DateTime Fecha { get; set; }
    public short IdCtUsuario { get; set; }
    public string Notas { get; set; } = null!;
    public OpPedidoEtt Nav_IdOpPedido { get; set; } = null!;
    public CtEstatusPedidoEtt Nav_IdCtEstatusPedido { get; set; } = null!;
    public CtUsuarioEtt Nav_IdCtUsuario { get; set; } = null!;
}