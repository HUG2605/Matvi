using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.Grupo.Commands;
using Matvi.Back.Application.Modules.Productos.Grupo.Dtos;
using Matvi.Back.Application.Modules.Productos.Grupo.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Productos.Pages.Grupo
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para details.
    /// </summary>
[AuthorizeMatvi(Funcion.Productos_Grupos)]
    public class DetailsModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa grupo.
        /// </summary>
public DetallarGrupoDto? Grupo { get; set; }

                /// <inheritdoc/>
public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idGrupo = ToByte(id);
            Grupo = await _mediator.Send(
                new DetallarGrupoQry(idGrupo), 
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
                Funcion.Productos_Grupos,
                Accion.Eliminar))
            {
                return new JsonResult(OperationResult<bool>.Error("Acceso denegado"));
            }

            var idGrupo = ToByte(id);
            var res = await _mediator.Send(
                new EliminarGrupoCmd(idGrupo), 
                cancellationToken);
            return new JsonResult(res);
        }
    }
}
