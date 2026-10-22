// ObtenerSaldoPendienteClienteQry.cs
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de obtener saldo pendiente cliente.
    /// </summary>
public record ObtenerSaldoPendienteClienteQry(
        byte IdSucursal,
        int IdCliente
    ) : IRequest<decimal>;
}