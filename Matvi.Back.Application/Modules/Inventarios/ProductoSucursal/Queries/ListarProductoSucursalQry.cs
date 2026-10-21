using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Queries
{
    public record ListarProductoSucursalQry(
        byte idSucursal,
        FilterGrd filter
        ) : IRequest<DataGrd<ListarProductoSucursalDto>>;
}
