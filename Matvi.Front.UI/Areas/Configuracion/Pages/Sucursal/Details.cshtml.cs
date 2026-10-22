using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Configuracion.Sucursal.Commands;
using Matvi.Back.Application.Modules.Configuracion.Sucursal.Dtos;
using Matvi.Back.Application.Modules.Configuracion.Sucursal.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Configuracion.Pages.Sucursal
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para details.
    /// </summary>
[AuthorizeMatvi(Funcion.Configuracion_Sucursales)]
    public class DetailsModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ) : BaseModel
    {

                /// <summary>
        /// Propiedad que representa sucursal.
        /// </summary>
public DetallarSucursalDto? Sucursal { get; set; }

                /// <inheritdoc/>
public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idSucursal = ToByte(id);
            Sucursal = await _mediator.Send(
                new DetallarSucursalQry(idSucursal), 
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
                Funcion.Configuracion_Sucursales,
                Accion.Eliminar))
            {
                return new JsonResult(OperationResult<bool>.Error("Acceso denegado"));
            }

            var idSucursal = ToByte(id);
            var res = await _mediator.Send(
                new EliminarSucursalCmd(idSucursal), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
