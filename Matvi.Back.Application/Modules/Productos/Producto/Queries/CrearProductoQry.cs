using Matvi.Back.Application.Modules.Productos.Producto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Producto.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de crear producto.
    /// </summary>
public record CrearProductoQry(
        ) : IRequest<CrearProductoDto>;   
}
