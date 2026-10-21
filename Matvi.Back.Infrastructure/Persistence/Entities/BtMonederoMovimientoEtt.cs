using Matvi.Back.Infrastructure.Common.Interceptors;
using System;
using System.Collections.Generic;

namespace Matvi.Back.Infrastructure.Persistence.Entities;

public class BtMonederoMovimientoEtt : IAuditable
{
    public int Id { get; set; }
    public int IdCtCliente { get; set; }
    public int? IdOpPedido { get; set; }
    public byte Tipo { get; set; }
    public decimal Monto { get; set; }
    public decimal SaldoRestante { get; set; }
    public DateTime Fecha { get; set; }
    public CtClienteEtt Nav_IdCtCliente { get; set; } = null!;
    public OpPedidoEtt? Nav_IdOpPedido { get; set; }
}