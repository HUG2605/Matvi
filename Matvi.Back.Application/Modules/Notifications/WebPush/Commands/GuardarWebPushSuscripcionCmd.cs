using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Notifications.WebPush.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Notifications.WebPush.Commands
{
    public record GuardarWebPushSuscripcionCmd(
        WebPushSuscripcionDto Suscripcion
        ) : IRequest<OperationResult<bool>>;
}
