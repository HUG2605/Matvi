using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Seguridad.ChangePassword.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Seguridad.ChangePassword.Commands
{
    public record CambiarContraseniaCmd(
        CambiarContraseniaDto usuario
        ) : IRequest<OperationResult<bool>>;
}
