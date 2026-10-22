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
    /// Modelo de página Razor (PageModel) que gestiona la interacción para create.
    /// </summary>
[AuthorizeMatvi(Funcion.Seguridad_Usuarios, Accion.Crear)]
    public class CreateModel(
        IMediator _mediator
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa usuario.
        /// </summary>
[BindProperty]
        public required CrearUsuarioDto Usuario { get; set; }

                /// <summary>
        /// Propiedad que representa roles select list.
        /// </summary>
public SelectList RolesSelectList { get; set; } = default!;

                /// <inheritdoc/>
public async Task OnGetAsync(
            CancellationToken cancellationToken)
        {
            var usuarioTask = _mediator.Send(
                new CrearUsuarioQry(), 
                cancellationToken);
            var rolesTask = _mediator.Send(
                new ListarCatalogoRolesQry(), 
                cancellationToken);

            await Task.WhenAll(usuarioTask, rolesTask);

            Usuario = usuarioTask.Result;
            RolesSelectList = new SelectList(rolesTask.Result, "Id", "Nombre");
        }

                /// <summary>
        /// Ejecuta la operación de on post create async.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnPostCreateAsync(
            CancellationToken cancellationToken)
        {
            var res = await _mediator.Send(
                new CrearUsuarioCmd(Usuario), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
