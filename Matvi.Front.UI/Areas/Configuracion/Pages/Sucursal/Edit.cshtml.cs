using Matvi.Back.Application.Modules.Configuracion.Sucursal.Commands;
using Matvi.Back.Application.Modules.Configuracion.Sucursal.Dtos;
using Matvi.Back.Application.Modules.Configuracion.Sucursal.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Configuracion.Pages.Sucursal
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para edit.
    /// </summary>
[AuthorizeMatvi(Funcion.Configuracion_Sucursales, Accion.Editar)]
    public class EditModel(
        IMediator _mediator
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa sucursal.
        /// </summary>
[BindProperty]
        public required EditarSucursalDto Sucursal { get; set; }

                /// <inheritdoc/>
public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idSucursal = ToByte(id);
            Sucursal = await _mediator.Send(
                new EditarSucursalQry(idSucursal), 
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
            Sucursal.Id = ToByte(Request.Form["Sucursal.SafeId"]!);
            var res = await _mediator.Send(
                new EditarSucursalCmd(Sucursal), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
