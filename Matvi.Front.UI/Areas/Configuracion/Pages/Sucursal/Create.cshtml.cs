using Matvi.Back.Application.Modules.Configuracion.Sucursal.Commands;
using Matvi.Back.Application.Modules.Configuracion.Sucursal.Dtos;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Configuracion.Pages.Sucursal
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para create.
    /// </summary>
[AuthorizeMatvi(Funcion.Configuracion_Sucursales, Accion.Crear)]
    public class CreateModel(
        IMediator _mediator
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa sucursal.
        /// </summary>
[BindProperty]
        public required CrearSucursalDto Sucursal { get; set; }

                /// <inheritdoc/>
public void OnGet()
        {
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
                new CrearSucursalCmd(Sucursal), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
