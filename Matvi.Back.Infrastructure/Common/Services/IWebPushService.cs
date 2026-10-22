using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Common.Services
{
    /// <summary>
    /// Servicio para el envío de notificaciones push web a navegadores y dispositivos suscritos.
    /// </summary>
    public interface IWebPushService
    {
        /// <summary>
        /// Envía una notificación WebPush a una lista de suscripciones activas usando el protocolo VAPID.
        /// </summary>
        /// <param name="suscripciones">Colección de suscripciones de usuario destino.</param>
        /// <param name="titulo">Título de la notificación emergente.</param>
        /// <param name="cuerpo">Texto o mensaje del cuerpo de la notificación.</param>
        /// <param name="datos">Datos adicionales u objeto de carga útil asociado a la notificación.</param>
        /// <param name="cancellationToken">Token de cancelación de la operación asíncrona.</param>
        /// <returns>Tarea que representa la finalización del envío.</returns>
        Task EnviarAsync(
            List<CtUsuarioWebPushSuscripcionEtt> suscripciones,
            string titulo,
            string cuerpo,
            object? datos,
            CancellationToken cancellationToken);
    }
}
