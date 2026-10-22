using Matvi.Back.Application.Modules.Productos.Producto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Producto.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de detallar producto.
    /// </summary>
public record DetallarProductoQry(
        int id
        ) : IRequest<DetallarProductoDto>;
}
