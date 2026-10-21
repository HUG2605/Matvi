// ObtenerSaldoPendienteClienteQry.cs
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Queries
{
    public record ObtenerSaldoPendienteClienteQry(
        byte IdSucursal,
        int IdCliente
    ) : IRequest<decimal>;
}