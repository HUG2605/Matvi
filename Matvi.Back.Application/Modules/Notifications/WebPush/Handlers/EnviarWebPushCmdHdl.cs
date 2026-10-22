using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Notifications.WebPush.Commands;
using Matvi.Back.Infrastructure.Common.Services;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Notifications.WebPush.Handlers
{
    class EnviarWebPushCmdHdl(
        IUnitOfWork _unitOfWork,
        IWebPushService _webPushService
    ) : IRequestHandler<EnviarWebPushCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            EnviarWebPushCmd request,
            CancellationToken cancellationToken)
        {
            var suscripciones = _unitOfWork
                .Query<CtUsuarioWebPushSuscripcionEtt>()
                .Where(s => s.Activa)
                .ToList();

            if (!suscripciones.Any())
                return OperationResult<bool>.Success(true, "Sin suscripciones activas.");

            await _webPushService.EnviarAsync(
                suscripciones,
                request.Titulo,
                request.Cuerpo,
                request.Datos,
                cancellationToken);

            return OperationResult<bool>.Success(true, "Notificación enviada.");
        }
    }
}
