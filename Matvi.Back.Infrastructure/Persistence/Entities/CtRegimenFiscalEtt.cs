using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtRegimenFiscalEtt : IAuditable
{
    public short Id { get; set; }
    public string Nombre { get; set; } = null!;
    public bool Fisica { get; set; }
    public bool Moral { get; set; }
    public ICollection<CtClienteEtt> Coll_CtCliente_IdCtRegimenFiscal { get; set; } = new List<CtClienteEtt>();
}