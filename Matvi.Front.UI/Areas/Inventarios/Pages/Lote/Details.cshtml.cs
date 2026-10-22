using Matvi.Back.Application.Modules.Inventarios.Lote.Dtos;
using Matvi.Back.Application.Modules.Inventarios.Lote.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Inventarios.Pages.Lote
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para details.
    /// </summary>
[AuthorizeMatvi(Funcion.Inventarios_Lotes)]
    public class DetailsModel(
        IMediator _mediator
    ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa lote.
        /// </summary>
public DetallarLoteDto? Lote { get; set; }

                /// <inheritdoc/>
public async Task<IActionResult> OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToPage("Index");

            var idLote = ToInt(id);
            Lote = await _mediator.Send(
                new DetallarLoteQry(idLote), 
                cancellationToken);

            if (Lote == null)
                return NotFound();

            return Page();
        }
    }
}

