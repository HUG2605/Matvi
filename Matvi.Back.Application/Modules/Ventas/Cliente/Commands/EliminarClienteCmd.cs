using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de eliminar cliente.
    /// </summary>
public record EliminarClienteCmd(
        int id
        ) : IRequest<OperationResult<bool>>;
}
