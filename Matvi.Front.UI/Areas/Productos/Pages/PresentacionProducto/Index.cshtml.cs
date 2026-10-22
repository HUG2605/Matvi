using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Commands;
using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Queries;
using Matvi.Front.UI.Areas.Productos.Pages.PresentacionProducto.ViewModels;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Productos.Pages.PresentacionProducto
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para index.
    /// </summary>
[AuthorizeMatvi(Funcion.Productos_Productos)]
    public class IndexModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ) : BaseModel
    {
                /// <summary>
        /// Ejecuta la operación de on get list async.
        /// </summary>
        /// <param name="productoId">Parámetro producto id.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<ActionResult> OnGetListAsync(
            string productoId, 
            CancellationToken cancellationToken)
        {
            var idProducto = ToInt(productoId);
            var vm = new ListarPresentacionProductoVm
            {
                CtProd_SafeId = productoId,
                PresentacionesProducto = await _mediator.Send(
                    new ListarPresentacionProductoGridQry(idProducto), 
                    cancellationToken)
            };
            return Partial("_List", vm);
        }

                /// <summary>
        /// Ejecuta la operación de on get delete.
        /// </summary>
        /// <param name="id">Identificador único del registro u objeto.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnGetDelete(
            string id, 
            CancellationToken cancellationToken)
        {
            if (!_usuarioActualService.TienePermiso(
                Funcion.Productos_Productos,
                Accion.Eliminar_PresentacionProducto))
            {
                return new JsonResult(OperationResult<bool>.Error("Acceso denegado"));
            }

            var idPresentacionProducto = ToInt(id);
            var res = await _mediator.Send(
                new EliminarPresentacionProductoCmd(idPresentacionProducto), 
                cancellationToken);

            return new JsonResult(res);
        }

    }
}
