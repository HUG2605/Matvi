using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Commands
{
    public record CancelarPedidoCmd(
        CancelarPedidoDto cancelar
    ) : IRequest<OperationResult<bool>>;
}
