using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.Cliente.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de crear cliente.
    /// </summary>
public record CrearClienteCmd(
          CrearClienteDto cliente
    ) : IRequest<OperationResult<bool>>;
}
