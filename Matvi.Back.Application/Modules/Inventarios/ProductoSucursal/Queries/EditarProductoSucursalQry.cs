using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Queries
{
    public record EditarProductoSucursalQry(
        int idProducto,
        byte idSucursal
        ) : IRequest<EditarProductoSucursalDto>;
}
