using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Productos.Producto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Producto.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar producto grid.
    /// </summary>
public record ListarProductoGridQry(
        FilterGrd filter
        ) : IRequest<DataGrd<ListarProductoDto>>;
}
