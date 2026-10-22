using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de eliminar usuario.
    /// </summary>
public record EliminarUsuarioCmd(
        short id
        ) : IRequest<OperationResult<bool>>;
}
