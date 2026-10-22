using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de editar producto sucursal.
    /// </summary>
public record EditarProductoSucursalQry(
        int idProducto,
        byte idSucursal
        ) : IRequest<EditarProductoSucursalDto>;
}
