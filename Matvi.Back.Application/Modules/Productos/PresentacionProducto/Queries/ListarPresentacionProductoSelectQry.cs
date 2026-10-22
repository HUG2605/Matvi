using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar presentacion producto select.
    /// </summary>
public record ListarPresentacionProductoSelectQry(
        int idProducto,
        bool paraCompra,
        bool paraVenta
        ) : IRequest<List<BaseDto<int>>>;
}
