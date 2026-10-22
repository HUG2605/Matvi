using Matvi.Back.Application.Modules.Productos.Producto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Producto.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de editar producto.
    /// </summary>
public record EditarProductoQry(
        int id
        ) : IRequest<EditarProductoDto>;
}
