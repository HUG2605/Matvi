using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Commands;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Dtos;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Seguridad.Pages.Usuario
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para details.
    /// </summary>
[AuthorizeMatvi(Funcion.Seguridad_Usuarios)]
    public class DetailsModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa usuario.
        /// </summary>
public DetallarUsuarioDto? Usuario { get; set; }

                /// <inheritdoc/>
public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idUsuario = ToShort(id);
            Usuario = await _mediator.Send(
                new DetallarUsuarioQry(idUsuario), 
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
                Funcion.Seguridad_Usuarios,
                Accion.Eliminar))
            {
                return new JsonResult(OperationResult<bool>.Error("Acceso denegado"));
            }

            var idUsuario = ToShort(id);
            var res = await _mediator.Send(
                new EliminarUsuarioCmd(idUsuario), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
