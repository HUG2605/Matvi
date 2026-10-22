using Matvi.Back.Application.Modules.Productos.Seccion.Commands;
using Matvi.Back.Application.Modules.Productos.Seccion.Dtos;
using Matvi.Back.Application.Modules.Productos.Seccion.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Productos.Pages.Seccion
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para edit.
    /// </summary>
[AuthorizeMatvi(Funcion.Productos_Secciones, Accion.Editar)]
    public class EditModel(
        IMediator _mediator
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa seccion.
        /// </summary>
[BindProperty]
        public required EditarSeccionDto Seccion { get; set; }

                /// <inheritdoc/>
public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idSeccion = ToByte(id);
            Seccion = await _mediator.Send(
                new EditarSeccionQry(idSeccion), 
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
            Seccion.Id = ToByte(Request.Form["Seccion.SafeId"]!);
            var res = await _mediator.Send(
                new EditarSeccionCmd(Seccion), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
