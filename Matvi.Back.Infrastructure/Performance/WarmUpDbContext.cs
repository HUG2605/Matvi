using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Matvi.Back.Infrastructure.Performance
{
    public static class WarmUpDbContext
    {
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
