using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Queries
{
    public record ListarPresentacionProductoSelectQry(
        int idProducto,
        bool paraCompra,
        bool paraVenta
        ) : IRequest<List<BaseDto<int>>>;
}
