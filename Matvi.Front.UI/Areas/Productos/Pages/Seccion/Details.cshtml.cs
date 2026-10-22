using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.Seccion.Commands;
using Matvi.Back.Application.Modules.Productos.Seccion.Dtos;
using Matvi.Back.Application.Modules.Productos.Seccion.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Productos.Pages.Seccion
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para details.
    /// </summary>
[AuthorizeMatvi(Funcion.Productos_Secciones)]
    public class DetailsModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa seccion.
        /// </summary>
public DetallarSeccionDto? Seccion { get; set; }

                /// <inheritdoc/>
public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idSeccion = ToByte(id);
            Seccion = await _mediator.Send(
                new DetallarSeccionQry(idSeccion), 
                cancellationToken);
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
                Funcion.Productos_Secciones,
                Accion.Eliminar))
            {
                return new JsonResult(OperationResult<bool>.Error("Acceso denegado"));
            }

            var idSeccion = ToByte(id);
            var res = await _mediator.Send(
                new EliminarSeccionCmd(idSeccion), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
