using Matvi.Back.Application.Modules.Compras.Proveedor.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Compras.Proveedor.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de detallar proveedor.
    /// </summary>
public record DetallarProveedorQry(
        int id
        ) : IRequest<DetallarProveedorDto>;
}
