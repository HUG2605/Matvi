using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Matvi.Back.Infrastructure.Performance
{
    /// <summary>
    /// Utilidad para inicializar y precalentar el modelo y conexión de Entity Framework Core durante el arranque de la aplicación.
    /// </summary>
    public static class WarmUpDbContext
    {
        /// <summary>
        /// Ejecuta una consulta inicial ligera para compilar el modelo de EF y abrir la primera conexión al iniciar la aplicación.
        /// </summary>
        /// <param name="scope">Ámbito de servicios de inyección de dependencias.</param>
        /// <returns>Tarea que representa la inicialización asíncrona.</returns>
        public static async Task InitializeAsync(IServiceScope scope)
        {
            var db = scope.ServiceProvider.GetRequiredService<MatviQryDbCtx>();

            _ = db.Model;

            await db.Set<CtUsuarioEtt>()
                    .AsNoTracking()
                    .Take(1)
                    .ToListAsync();

        }
    }
}
