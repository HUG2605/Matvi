using Matvi.Back.Application.Modules.Ventas.Cliente.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de detallar ciente.
    /// </summary>
public record DetallarCienteQry(
        int id
        ) : IRequest<DetallarClienteDto>;
}
