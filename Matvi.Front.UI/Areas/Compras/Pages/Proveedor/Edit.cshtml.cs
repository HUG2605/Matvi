using Matvi.Back.Application.Modules.Compras.Proveedor.Commands;
using Matvi.Back.Application.Modules.Compras.Proveedor.Dtos;
using Matvi.Back.Application.Modules.Compras.Proveedor.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Compras.Pages.Proveedor
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para edit.
    /// </summary>
[AuthorizeMatvi(Funcion.Compras_Proveedores, Accion.Editar)]
    public class EditModel(
        IMediator _mediator
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa proveedor.
        /// </summary>
[BindProperty]
        public required EditarProveedorDto Proveedor { get; set; }

                /// <inheritdoc/>
public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idProveedor = ToInt(id);
            Proveedor = await _mediator.Send(
                new EditarProveedorQry(idProveedor), 
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
            Proveedor.Id = ToInt(Request.Form["Proveedor.SafeId"]!);
            var res = await _mediator.Send(
                new EditarProveedorCmd(Proveedor), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
