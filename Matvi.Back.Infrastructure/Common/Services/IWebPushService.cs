using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Infrastructure.Common.Services
{
    public interface IWebPushService
    {
        Task EnviarAsync(
            List<CtUsuarioWebPushSuscripcionEtt> suscripciones,
            string titulo,
            string cuerpo,
            object? datos,
            CancellationToken cancellationToken);
    }
}
