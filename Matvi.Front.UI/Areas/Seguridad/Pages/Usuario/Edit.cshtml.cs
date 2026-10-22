using Matvi.Back.Application.Modules.Seguridad.Rol.Queries;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Commands;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Dtos;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Matvi.Front.UI.Areas.Seguridad.Pages.Usuario
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para edit.
    /// </summary>
[AuthorizeMatvi(Funcion.Seguridad_Usuarios, Accion.Editar)]
    public class EditModel(
        IMediator _mediator
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa usuario.
        /// </summary>
[BindProperty]
        public required EditarUsuarioDto Usuario { get; set; }

                /// <summary>
        /// Propiedad que representa roles select list.
        /// </summary>
public SelectList RolesSelectList { get; set; } = default!;

                /// <inheritdoc/>
public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idUsuario = ToShort(id);

            var usuarioTask = _mediator.Send(
                new EditarUsuarioQry(idUsuario), 
                cancellationToken);
            var rolesTask = _mediator.Send(
                new ListarCatalogoRolesQry(), 
                cancellationToken);

            await Task.WhenAll(usuarioTask, rolesTask);

            Usuario = usuarioTask.Result;
            RolesSelectList = new SelectList(rolesTask.Result, "Id", "Nombre");
        }

                /// <summary>
        /// Ejecuta la operación de on post edit async.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnPostEditAsync(
            CancellationToken cancellationToken)
        {
            Usuario.Id = ToShort(Request.Form["Usuario.SafeId"]!);
            var res = await _mediator.Send(
                new EditarUsuarioCmd(Usuario), 
                cancellationToken);
            return new JsonResult(res);
        }
    }
}
