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
    /// Modelo de página Razor (PageModel) que gestiona la interacción para create.
    /// </summary>
[AuthorizeMatvi(Funcion.Productos_Productos, Accion.Crear_PresentacionProducto)]
    public class CreateModel(
        IMediator _mediator
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa presentacion producto.
        /// </summary>
[BindProperty]
        public required CrearPresentacionProductoDto PresentacionProducto { get; set; }

                /// <inheritdoc/>
public async Task OnGetAsync(
            string productoId, 
            CancellationToken cancellationToken)
        {
            var idProducto = ToInt(productoId);
            PresentacionProducto = await _mediator.Send(
                new CrearPresentacionProductoQry(idProducto), 
                cancellationToken);
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
                new CrearPresentacionProductoCmd(PresentacionProducto), 
                cancellationToken);
            return new JsonResult(res);
        }
    }
}
