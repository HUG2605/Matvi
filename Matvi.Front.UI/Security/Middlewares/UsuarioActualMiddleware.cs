using Matvi.Front.UI.Constants;
using Matvi.Front.UI.Security.Services;
using Microsoft.AspNetCore.Authentication;

namespace Matvi.Front.UI.Security.Middlewares
{
        /// <summary>
    /// Representa la clase para usuario actual middleware.
    /// </summary>
public class UsuarioActualMiddleware
    {
        private readonly RequestDelegate _next;

                /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="UsuarioActualMiddleware"/>.
        /// </summary>
        /// <param name="next">Parámetro next.</param>
public UsuarioActualMiddleware(RequestDelegate next)
        {
            _next = next;
        }

                /// <summary>
        /// Ejecuta la operación de invoke async.
        /// </summary>
        /// <param name="context">Parámetro context.</param>
        /// <param name="usuarioActualService">Parámetro usuario actual service.</param>
public async Task InvokeAsync(HttpContext context, IUsuarioActualService usuarioActualService)
        {
            var path = context.Request.Path.Value?.ToLower() ?? "";

            // Ignorar recursos estáticos o rutas específicas
            var extensionesEstaticas = new[] { ".js", ".css", ".png", ".jpg", ".jpeg", ".svg", ".ico", ".woff", ".woff2", ".ttf", ".map" };

            if (extensionesEstaticas.Any(ext => path.EndsWith(ext)))
            {
                await _next(context);
                return;
            }

            // Opcional: ignorar también rutas como /favicon.ico o /static/
            if (path.StartsWith("/favicon") || path.StartsWith("/static") || path.StartsWith("/fonts"))
            {
                await _next(context);
                return;
            }

            // Solo ejecutar si el request apunta a una Razor Page u endpoint visual
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                await usuarioActualService.InicializarAsync();
                //var usuario = usuarioActualService.UsuarioSesion;
                if (usuarioActualService.EstaActivo == false)
                {
                    await context.SignOutAsync(AuthConstants.Scheme);
                    context.Session.Clear();
                    context.Response.Redirect("/Security/login");
                    return;
                }

            }
            await _next(context);
        }
    }
}