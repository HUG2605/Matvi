using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de obtener presentacion producto.
    /// </summary>
public record ObtenerPresentacionProductoQry(
        byte idSucursal,
        int idPresentacionProducto
        ) : IRequest<ObtenerPresentacionProductoDto>;
}
