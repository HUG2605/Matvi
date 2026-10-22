using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de cancelar pedido.
    /// </summary>
public record CancelarPedidoCmd(
        CancelarPedidoDto cancelar
    ) : IRequest<OperationResult<bool>>;
}
