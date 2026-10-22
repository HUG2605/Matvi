using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Commands;
using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Dtos;
using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Productos.Pages.PresentacionProducto
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para edit.
    /// </summary>
[AuthorizeMatvi(Funcion.Productos_Productos, Accion.Editar_PresentacionProducto)]
    public class EditModel(
        IMediator _mediator
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa presentacion producto.
        /// </summary>
[BindProperty]
        public required EditarPresentacionProductoDto PresentacionProducto { get; set; }

                /// <inheritdoc/>
public async Task OnGetAsync(
            string id, 
            CancellationToken cancellationToken)
        {
            var idPresentacionProducto = ToInt(id);
            PresentacionProducto = await _mediator.Send(
                new EditarPresentacionProductoQry(idPresentacionProducto), 
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
            PresentacionProducto.Id = ToInt(Request.Form["PresentacionProducto.SafeId"]!);
            var res = await _mediator.Send(
                new EditarPresentacionProductoCmd(PresentacionProducto), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
