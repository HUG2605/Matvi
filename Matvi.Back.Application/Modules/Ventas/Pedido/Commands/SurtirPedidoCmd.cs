using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de surtir pedido.
    /// </summary>
public record SurtirPedidoCmd(
        SurtirPedidoDto pedido
        ) : IRequest<OperationResult<bool>>;
}
