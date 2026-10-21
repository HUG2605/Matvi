using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Queries
{
    public record ListarPresentacionProductoGridQry(
        int idProducto
        ) : IRequest<List<ListarProductoSucursalDto>>;
}
