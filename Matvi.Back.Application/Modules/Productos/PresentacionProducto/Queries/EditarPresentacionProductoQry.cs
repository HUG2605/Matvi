using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de editar presentacion producto.
    /// </summary>
public record EditarPresentacionProductoQry(
        int id
        ) : IRequest<EditarPresentacionProductoDto>;
}
