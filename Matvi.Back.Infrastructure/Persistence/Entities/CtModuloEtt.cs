using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtModuloEtt : IAuditable
{
    public byte Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string IconName { get; set; } = null!;
    public string Area { get; set; } = null!;
    public ICollection<CtFuncionEtt> Coll_CtFuncion_IdCtModulo { get; set; } = new List<CtFuncionEtt>();
}