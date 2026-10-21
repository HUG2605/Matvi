using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Notifications.WebPush.Commands
{
    public record EnviarWebPushCmd(
        string Titulo,
        string Cuerpo,
        object? Datos
    ) : IRequest<OperationResult<bool>>;
}
