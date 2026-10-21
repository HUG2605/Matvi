using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtRolEtt : IAuditable
{
    public byte Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Acronimo { get; set; } = null!;
    public ICollection<RlRolFuncionEtt> Coll_RlRolFuncion_IdCtRol { get; set; } = new List<RlRolFuncionEtt>();
    public ICollection<RlRolAccionEtt> Coll_RlRolAccion_IdCtRol { get; set; } = new List<RlRolAccionEtt>();
    public ICollection<CtUsuarioEtt> Coll_CtUsuario_IdCtRol { get; set; } = new List<CtUsuarioEtt>();
}