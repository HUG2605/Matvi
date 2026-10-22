using Matvi.Back.Application.Modules.Seguridad.Rol.Commands;
using Matvi.Back.Application.Modules.Seguridad.Rol.Dtos;
using Matvi.Back.Application.Modules.Seguridad.Rol.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Seguridad.Pages.Rol
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para edit.
    /// </summary>
[AuthorizeMatvi(Funcion.Seguridad_Roles, Accion.Editar)]
    public class EditModel(
        IMediator _mediator
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa rol.
        /// </summary>
[BindProperty]
        public required EditarRolDto Rol { get; set; }

                /// <inheritdoc/>
public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idRol = ToByte(id);
            Rol = await _mediator.Send(
                new EditarRolQry(idRol), 
                cancellationToken);
        }

                /// <summary>
        /// Ejecuta la operación de on post edit async.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnPostEditAsync(
            CancellationToken cancellationToken)
        {
            Rol.Id = ToShort(Request.Form["Rol.SafeId"]!);
            var res = await _mediator.Send(
                new EditarRolCmd(Rol), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
