using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Productos.Producto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Producto.Queries
{
    public record ListarProductoGridQry(
        FilterGrd filter
        ) : IRequest<DataGrd<ListarProductoDto>>;
}
