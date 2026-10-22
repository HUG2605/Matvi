using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Compras.Compra.Queries;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Commands;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Dtos;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.Gasto
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para details.
    /// </summary>
[AuthorizeMatvi(Funcion.Finanzas_Gastos)]
    public class DetailsModel(
        IUsuarioActualService _usuarioActualService,
        IMediator _mediator
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa gasto.
        /// </summary>
public DetallarGastoDto? Gasto { get; set; }
                /// <inheritdoc/>
public async Task<IActionResult> OnGet(
            string id,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToPage("Index");

            var idCompra = ToInt(id);
            Gasto = await _mediator.Send(
                new DetallarGastoQry(idCompra),
                cancellationToken);

            if (Gasto == null)
                return NotFound();

            return Page();

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
                Funcion.Finanzas_Gastos,
                Accion.Eliminar))
            {
                return new JsonResult(OperationResult<bool>.Error("Acceso denegado"));

            }

            var idCompra = ToInt(id);
            var res = await _mediator.Send(
                new EliminarGastoCmd(idCompra),
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
