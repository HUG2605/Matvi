using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Commands
{
    public record EliminarUsuarioCmd(
        short id
        ) : IRequest<OperationResult<bool>>;
}
