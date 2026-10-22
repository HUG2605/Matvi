using Matvi.Back.Application.Modules.Inventarios.Movimiento.Dtos;
using Matvi.Back.Application.Modules.Inventarios.Movimiento.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Inventarios.Pages.Movimiento
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para details.
    /// </summary>
[AuthorizeMatvi(Funcion.Inventarios_Movimientos)]
    public class DetailsModel(
        IMediator _mediator
    ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa movimiento.
        /// </summary>
public DetalleMovimientoDto? Movimiento { get; set; }

                /// <inheritdoc/>
public async Task<IActionResult> OnGet(
            string id,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToPage("Index");

            var idMovimiento = ToInt(id);
            Movimiento = await _mediator.Send(
                new DetallarMovimientoQry(idMovimiento),
                cancellationToken);

            if (Movimiento == null)
                return NotFound();

            return Page();
        }
    }
}