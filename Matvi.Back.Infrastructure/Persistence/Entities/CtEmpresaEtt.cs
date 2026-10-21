using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtEmpresaEtt : IAuditable
{
    public byte Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Logotipo { get; set; }
    public string? Telefono { get; set; }
    public ICollection<CtSucursalEtt> Coll_CtSucursal_IdCtEmpresa { get; set; } = new List<CtSucursalEtt>();
}