using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtAccionEtt : IAuditable
{
    public short Id { get; set; }
    public byte IdCtFuncion { get; set; }
    public string Nombre { get; set; } = null!;
    public CtFuncionEtt Nav_IdCtFuncion { get; set; } = null!;
    public ICollection<RlRolAccionEtt> Coll_RlRolAccion_IdCtAccion { get; set; } = new List<RlRolAccionEtt>();
}