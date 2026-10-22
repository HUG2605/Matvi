using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Seguridad.ChangePassword.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Seguridad.ChangePassword.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de cambiar contrasenia.
    /// </summary>
public record CambiarContraseniaCmd(
        CambiarContraseniaDto usuario
        ) : IRequest<OperationResult<bool>>;
}
