using Matvi.Back.Application.Modules.Ventas.Cliente.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de obtener estado credito cliente.
    /// </summary>
public record ObtenerEstadoCreditoClienteQry(
        int IdCliente
    ) : IRequest<EstadoCreditoClienteDto?>;
}
