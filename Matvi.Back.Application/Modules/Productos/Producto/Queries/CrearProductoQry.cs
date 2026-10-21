using Matvi.Back.Application.Modules.Productos.Producto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Producto.Queries
{
    public record CrearProductoQry(
        ) : IRequest<CrearProductoDto>;   
}
