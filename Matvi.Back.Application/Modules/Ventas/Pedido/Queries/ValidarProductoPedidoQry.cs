using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Queries
{
    public record ValidarProductoPedidoQry(
        byte idSucursal,
        int idPresentacionProducto,
        short cantidad
        ) : IRequest<OperationResult<AgregarProductoPedidoDto?>>;
}