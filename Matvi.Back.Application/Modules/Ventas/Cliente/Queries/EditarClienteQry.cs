using Matvi.Back.Application.Modules.Ventas.Cliente.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de editar cliente.
    /// </summary>
public record EditarClienteQry(
        int id
        ) : IRequest<EditarClienteDto>;
}
