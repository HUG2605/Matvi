using Matvi.Back.Application.Modules.Compras.Proveedor.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Compras.Proveedor.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de editar proveedor.
    /// </summary>
public record EditarProveedorQry(
        int id
        ) : IRequest<EditarProveedorDto>;

}
