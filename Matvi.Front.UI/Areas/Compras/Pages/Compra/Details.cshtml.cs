using Matvi.Back.Application.Modules.Compras.Compra.Dtos;
using Matvi.Back.Application.Modules.Compras.Compra.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Compras.Pages.Compra
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para details.
    /// </summary>
[AuthorizeMatvi(Funcion.Compras_Compras)]
    public class DetailsModel(
        IMediator _mediator
    ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa compra.
        /// </summary>
public DetallarCompraDto? Compra { get; set; }

                /// <inheritdoc/>
public async Task<IActionResult> OnGet(
            string id,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToPage("Index");

            var idCompra = ToInt(id);
            Compra = await _mediator.Send(
                new DetallarCompraQry(idCompra),
                cancellationToken);

            if (Compra == null)
                return NotFound();

            return Page();
        }
    }
}