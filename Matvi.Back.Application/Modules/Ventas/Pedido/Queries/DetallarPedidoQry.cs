using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Queries
{
    public record DetallarPedidoQry(int Id, bool puedeVerSensibles) :
        IRequest<DetallarPedidoDto>;
}