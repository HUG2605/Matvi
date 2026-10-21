using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Commands
{
    public record RegistrarEventoPedidoCmd(
        RegistrarEventoPedidoDto evento
    ) : IRequest<OperationResult<long>>;
}