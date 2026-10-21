using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class CtCuentaBancariaEtt : IAuditable
{
    public byte Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Banco { get; set; } = null!;
    public string NumeroCuenta { get; set; } = null!;
    public bool Activa { get; set; }
    public ICollection<OpMovimientoBancarioEtt> Coll_OpMovimientoBancario_IdCtCuentaBancaria { get; set; } = new List<OpMovimientoBancarioEtt>();
}