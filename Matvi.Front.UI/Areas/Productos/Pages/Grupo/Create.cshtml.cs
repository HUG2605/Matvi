using Matvi.Back.Application.Modules.Productos.Grupo.Commands;
using Matvi.Back.Application.Modules.Productos.Grupo.Dtos;
using Matvi.Back.Application.Modules.Productos.Seccion.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Matvi.Front.UI.Areas.Productos.Pages.Grupo
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para create.
    /// </summary>
[AuthorizeMatvi(Funcion.Productos_Grupos, Accion.Crear)]
    public class CreateModel(
        IMediator _mediator
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa grupo.
        /// </summary>
[BindProperty]
        public required CrearGrupoDto Grupo { get; set; }

                /// <summary>
        /// Propiedad que representa seccion select list.
        /// </summary>
public SelectList SeccionSelectList { get; set; } = default!;

                /// <inheritdoc/>
public async Task OnGetAsync(
            CancellationToken cancellationToken)
        {
            var seccionList = await _mediator.Send(
                new ListarSeccionSelectQry(), 
                cancellationToken);

            SeccionSelectList = new SelectList(seccionList, "Id", "Nombre");
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
                new CrearGrupoCmd(Grupo), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
