using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Notifications.WebPush.Commands;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Notifications.WebPush.Handlers
{
    class GuardarWebPushSuscripcionCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<GuardarWebPushSuscripcionCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            GuardarWebPushSuscripcionCmd request,
            CancellationToken cancellationToken)
        {
            var dto = request.Suscripcion;

            // Si ya existe ese endpoint para este usuario, solo reactivar
            var existente = _unitOfWork
                .Query<CtUsuarioWebPushSuscripcionEtt>()
                .FirstOrDefault(s => s.IdCtUsuario == request.Suscripcion.CtUsua_Id
                                  && s.Endpoint == dto.Endpoint);

            if (existente != null)
            {
                existente.P256dh = dto.Keys.P256dh;
                existente.Auth = dto.Keys.Auth;
                existente.FechaRegistro = DateTime.Now;
                existente.Activa = true;
                _unitOfWork.Update(existente);
            }
            else
            {
                var nueva = new CtUsuarioWebPushSuscripcionEtt
                {
                    IdCtUsuario = request.Suscripcion.CtUsua_Id,
                    Endpoint = dto.Endpoint,
                    P256dh = dto.Keys.P256dh,
                    Auth = dto.Keys.Auth,
                    FechaRegistro = DateTime.Now,
                    Activa = true
                };
                _unitOfWork.Add(nueva);
            }

            await _unitOfWork.SaveChangesAsync(CancellationToken.None);

            return OperationResult<bool>.Success(true, "Suscripción registrada.");
        }
    }
}
