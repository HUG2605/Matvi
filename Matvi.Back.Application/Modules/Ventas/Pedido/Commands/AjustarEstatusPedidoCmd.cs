using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de ajustar estatus pedido.
    /// </summary>
public record AjustarEstatusPedidoCmd(
        AjustarEstatusPedidoDto ajuste
    ) : IRequest<OperationResult<bool>>;
}