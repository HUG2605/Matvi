using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de editar usuario.
    /// </summary>
public record EditarUsuarioCmd(
        EditarUsuarioDto usuario
        ) : IRequest<OperationResult<bool>>;
}
