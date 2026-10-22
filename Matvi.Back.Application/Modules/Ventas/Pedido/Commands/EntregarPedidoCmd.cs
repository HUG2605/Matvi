using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de entregar pedido.
    /// </summary>
public record EntregarPedidoCmd(
        EntregarPedidoDto entrega
        ) : IRequest<OperationResult<bool>>;
}