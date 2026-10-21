using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtFuncionEtt : IAuditable
{
    public byte Id { get; set; }
    public byte IdCtModulo { get; set; }
    public string Nombre { get; set; } = null!;
    public string Ruta { get; set; } = null!;
    public CtModuloEtt Nav_IdCtModulo { get; set; } = null!;
    public ICollection<CtAccionEtt> Coll_CtAccion_IdCtFuncion { get; set; } = new List<CtAccionEtt>();
    public ICollection<RlRolFuncionEtt> Coll_RlRolFuncion_IdCtFuncion { get; set; } = new List<RlRolFuncionEtt>();
}