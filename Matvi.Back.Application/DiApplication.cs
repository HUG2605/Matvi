using Matvi.Back.Application.Modules.Ventas.Pedido.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Matvi.Back.Application
{
        /// <summary>
    /// Representa la clase para di application.
    /// </summary>
public static class DiApplication
    {
                /// <summary>
        /// Crea o registra una nueva entrada de add matvi services.
        /// </summary>
        /// <param name="services">Parámetro services.</param>
        /// <param name="connectionString">Parámetro connection string.</param>
        /// <param name="userFactory">Parámetro user factory.</param>
        /// <returns>Objeto con la información resultante de add matvi services.</returns>
public static IServiceCollection AddMatviServices(
            this IServiceCollection services,
            string connectionString,
            Func<IServiceProvider, short> userFactory)
        {
            // MediatR: registra todos los handlers desde Aplicacion
            services.AddApplication();

            // Infraestructura: DbContext, UoW, repositorios, interceptores
            services.AddInfrastructure(connectionString, userFactory);

            return services;
        }

                /// <summary>
        /// Crea o registra una nueva entrada de add application.
        /// </summary>
        /// <param name="services">Parámetro services.</param>
        /// <returns>Objeto con la información resultante de add application.</returns>
public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Registro de MediatR y Validadores
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DiApplication).Assembly));

            // Handler invocado por inyección directa (no por MediatR) desde
            // CrearPedidoCmdHdl. Es necesario porque su atributo [Transaction]
            // hace que el TransactionalCmdHandlerDecorator commitee la
            // transacción compartida si se invoca vía _sender.Send desde
            // dentro de otro handler transaccional. Por eso se invoca
            // directo, y por eso se debe registrar aquí.
            services.AddScoped<AjustarEstatusPedidoCmdHdl>();

            return services;
        }

    }

}
