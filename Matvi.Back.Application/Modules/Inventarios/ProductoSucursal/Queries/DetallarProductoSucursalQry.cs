using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de detallar producto sucursal.
    /// </summary>
public record DetallarProductoSucursalQry(
        int idProducto,
        byte idSucursal
        ) : IRequest<DetallarProductoSucursalDto>;
}
