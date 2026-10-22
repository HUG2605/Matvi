using Matvi.Back.Infrastructure.Persistence.Entities;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text.Json;
using WebPush;

namespace Matvi.Back.Infrastructure.Common.Services
{
    /// <summary>
    /// Implementación de <see cref="IWebPushService"/> basada en la biblioteca WebPush y llaves VAPID configuradas.
    /// </summary>
    public class WebPushService(IConfiguration _config) : IWebPushService
    {
        /// <inheritdoc/>
        public async Task EnviarAsync(
            List<CtUsuarioWebPushSuscripcionEtt> suscripciones,
            string titulo,
            string cuerpo,
            object? datos,
            CancellationToken cancellationToken)
        {
            var client = new WebPushClient();

            var vapidDetails = new VapidDetails(
                _config["Vapid:Subject"],
                _config["Vapid:PublicKey"],
                _config["Vapid:PrivateKey"]);

            var payload = JsonSerializer.Serialize(new { titulo, cuerpo, datos });

            foreach (var sus in suscripciones)
            {
                try
                {
                    var subscription = new PushSubscription(
                        sus.Endpoint,
                        sus.P256dh,
                        sus.Auth);

                    await client.SendNotificationAsync(
                        subscription, payload, vapidDetails);
                }
                catch (WebPushException ex)
                    when (ex.StatusCode == HttpStatusCode.Gone
                       || ex.StatusCode == HttpStatusCode.NotFound)
                {
                    // Suscripción expirada → desactivar
                    sus.Activa = false;
                }
            }
        }
    }
}
