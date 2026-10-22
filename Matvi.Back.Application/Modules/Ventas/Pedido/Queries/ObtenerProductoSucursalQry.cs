using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de obtener producto sucursal.
    /// </summary>
public record ObtenerProductoSucursalQry(
        byte idCtSucursal,
        int idProducto
        ) : IRequest<ObtenerProductoSucursalDto>;
}
