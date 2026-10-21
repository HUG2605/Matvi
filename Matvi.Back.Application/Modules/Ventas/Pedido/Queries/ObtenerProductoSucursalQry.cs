using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Queries
{
    public record ObtenerProductoSucursalQry(
        byte idCtSucursal,
        int idProducto
        ) : IRequest<ObtenerProductoSucursalDto>;
}
