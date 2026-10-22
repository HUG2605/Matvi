using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de validar producto pedido.
    /// </summary>
public record ValidarProductoPedidoQry(
        byte idSucursal,
        int idPresentacionProducto,
        short cantidad
        ) : IRequest<OperationResult<AgregarProductoPedidoDto?>>;
}