using Matvi.Back.Application.Common.Extensions;
using Matvi.Back.Application.Modules.Seguridad.Login.Dtos;
using Matvi.Back.Application.Modules.Seguridad.Login.Queries;
using Matvi.Front.UI.Constants;
using Matvi.Front.UI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Matvi.Front.UI.Pages.Security
{
    [AllowAnonymous]
    public class LoginModel(
        IMediator _mediator
        ) : PageModel
    {
        [BindProperty]
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "Se requiere valor para {0}.")]
        public string? UsuarioFld { get; set; }

        [BindProperty]
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Se requiere valor para {0}.")]
        [DataType(DataType.Password)]
        public string? ContraseniaFld { get; set; }

        [BindProperty]
        [Display(Name = "Recordarme")]
        public bool Recordarme { get; set; }

        public void OnGet()
        {
            var password = Convert.ToString("AAJR3423").ToObfuscated();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            #region Forzar cerrado de sesión anterior, por si le dieron regresar al navegador

            await HttpContext.SignOutAsync(AuthConstants.Scheme);
            HttpContext.Session.Clear();

            #endregion

            if (UsuarioFld != null && ContraseniaFld != null)
            {
                var usuarioSesion = await _mediator.Send(
                    new ObtenerUsuarioSesionLoginQry(
                        UsuarioFld,
                        ContraseniaFld.ToObfuscated()), 
                    cancellationToken);

                if (usuarioSesion != null)
                {
                    if (UsuarioFld == ContraseniaFld)
                    {
                        TempData["UsuarioId"] = usuarioSesion.Id.ToString();
                        TempData["Usuario"] = UsuarioFld;
                        return RedirectToPage("ChangePassword");
                    }

                    usuarioSesion.Acciones = await _mediator.Send(
                        new ObtenerAccionesRolQry(usuarioSesion.RolId!.Value), 
                        cancellationToken);

                    await Prv_SignInUser(usuarioSesion, Recordarme);
            
                    return RedirectToPage("/Index");
                }
                else
                {
                    ModelState.AddModelError("", "Los datos ingresados son incorrectos, o el usuario está desactivado.");
                }
            }

            return Page();
        }

        #region Privates Methods

        async Task Prv_SignInUser(
            UsuarioLoginDto usuarioLogin, 
            bool recordarme)
        {
            var claims = new List<Claim>
            {
                new Claim(AuthConstants.UserSessionIdKey, usuarioLogin.Id.ToString())
            };

            foreach (var func in usuarioLogin.Funciones!)
            {
                claims.Add(new Claim("permiso", $"{func.Nombre}:Consultar"));
                if (func.VerSensibles)
                    claims.Add(new Claim($"verSensibles:{func.Nombre}", "true"));
            }

            foreach (var acc in usuarioLogin.Acciones!)
            {
                claims.Add(new Claim("permiso", $"{acc.Funcion}:{acc.Accion}"));
            }

            var identity = new ClaimsIdentity(claims, AuthConstants.Scheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = recordarme,
                ExpiresUtc = recordarme ? DateTimeOffset.UtcNow.AddDays(7) : null
            };

            await HttpContext.SignInAsync(AuthConstants.Scheme, principal, authProperties);

            if (usuarioLogin.SucursalSesion == null && usuarioLogin.Sucursales?.Count > 0)
            {
                usuarioLogin.SucursalSesion = usuarioLogin.Sucursales.First();
            }

            usuarioLogin.Acciones = null;
            HttpContext.Session.SetObject(AuthConstants.UserSessionKey, usuarioLogin);
        }

        #endregion

    }
}