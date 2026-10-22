using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.Cliente.Commands;
using Matvi.Back.Application.Modules.Ventas.Cliente.Dtos;
using Matvi.Back.Application.Modules.Ventas.Cliente.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Ventas.Pages.Cliente
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para details.
    /// </summary>
[AuthorizeMatvi(Funcion.Ventas_Clientes)]
    public class DetailsModel(
        IUsuarioActualService _usuarioActualService,
        IMediator _mediator
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa cliente.
        /// </summary>
public DetallarClienteDto? Cliente { get; set; }

                /// <inheritdoc/>
public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idCliente = ToInt(id);
            Cliente = await _mediator.Send(
                new DetallarCienteQry(idCliente), 
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
                Funcion.Ventas_Clientes,
                Accion.Eliminar))
            {
                return new JsonResult(OperationResult<bool>.Error("Acceso denegado"));
            }

            var idCliente = ToInt(id);
            var res = await _mediator.Send(
                new EliminarClienteCmd(idCliente), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
