using Matvi.Back.Application.Modules.Seguridad.ChangePassword.Commands;
using Matvi.Back.Application.Modules.Seguridad.ChangePassword.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Front.UI.Pages.Security
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para change password.
    /// </summary>
[AllowAnonymous]
    public class ChangePasswordModel (
        IMediator _mediator
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa usuario.
        /// </summary>
[Display(Name = "Usuario")]
        public string UsuarioFld { get; set; } = string.Empty;

                /// <summary>
        /// Propiedad que representa usuario.
        /// </summary>
[BindProperty]
        public required CambiarContraseniaDto Usuario { get; set; }

                /// <inheritdoc/>
public void OnGet()
        {
            Usuario = new CambiarContraseniaDto();
            Usuario.Id = short.Parse(TempData["UsuarioId"]?.ToString()!);
            UsuarioFld = TempData["Usuario"]!.ToString()!;
            TempData["UsuarioId"] = Usuario.Id.ToString();
            TempData["Usuario"] = UsuarioFld;
        }

                /// <inheritdoc/>
public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            Usuario.Id = ToShort(Request.Form["Usuario.SafeId"]!);
            var res = await _mediator.Send(
                new CambiarContraseniaCmd(Usuario), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
