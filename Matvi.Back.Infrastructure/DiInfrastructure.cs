using Matvi.Back.Infrastructure.Common.Decorators;
using Matvi.Back.Infrastructure.Common.Interceptors;
using Matvi.Back.Infrastructure.Common.Services;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Matvi.Back.Application
{
    public static class DiInfrastructure
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string connectionString,
            Func<IServiceProvider, short> userFactory)
        {
            // 1. Registrar el Func<short> que necesita el interceptor
            services.AddScoped<Func<short>>(sp => () => userFactory(sp));

            // 2. Registrar el interceptor
            services.AddScoped<AuditInterceptor>();

            // 3. Registrar DbContextFactory (SIN interceptores) → CONSULTAS
            services.AddDbContextFactory<MatviQryDbCtx>(options =>
            {
                options.UseSqlServer(connectionString)
                       .EnableSensitiveDataLogging();
            });

            // 4. Registrar DbContext scoped (CON interceptores) → COMANDOS + WARMUP
            services.AddDbContext<MatviCmdDbCtx>((provider, options) =>
            {
                var interceptor = provider.GetRequiredService<AuditInterceptor>();

                options.UseSqlServer(connectionString)
                       .EnableSensitiveDataLogging()
                       .AddInterceptors(interceptor);
            });

            // 5. UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // 6. FolioService
            services.AddScoped<IConsecutivoService, ConsecutivoService>();
            services.AddScoped<IWebPushService, WebPushService>();

            // 7. Decorador transaccional
            services.Decorate(typeof(IRequestHandler<,>), typeof(TransactionalCmdHandlerDecorator<,>));

            return services;
        }
    }
}