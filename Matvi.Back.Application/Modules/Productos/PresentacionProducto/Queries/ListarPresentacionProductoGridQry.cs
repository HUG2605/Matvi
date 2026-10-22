using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar presentacion producto grid.
    /// </summary>
public record ListarPresentacionProductoGridQry(
        int idProducto
        ) : IRequest<List<ListarProductoSucursalDto>>;
}
