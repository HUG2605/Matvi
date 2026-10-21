using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Matvi.Front.UI.Attributes
{
    public class AuthorizeMatviAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _funcion;
        private readonly string? _accion;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="funcion">[Nombre] de "CatFuncion"</param>
        /// <param name="accion">[Nombre] de CatAccion (default = Consultar)</param>
        public AuthorizeMatviAttribute(string funcion, string? accion = null)
        {
            _funcion = funcion;
            _accion = accion;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            // Validar consulta de función
            if (!user.HasClaim("permiso", $"{_funcion}:Consultar"))
            {
                context.Result = new ForbidResult();
                return;
            }

            // Validar acción específica, si aplica
            if (_accion is not null &&
                !user.HasClaim("permiso", $"{_funcion}:{_accion}"))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }

}
