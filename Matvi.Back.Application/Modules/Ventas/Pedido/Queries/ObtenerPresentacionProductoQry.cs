using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Queries
{
    public record ObtenerPresentacionProductoQry(
        byte idSucursal,
        int idPresentacionProducto
        ) : IRequest<ObtenerPresentacionProductoDto>;
}
