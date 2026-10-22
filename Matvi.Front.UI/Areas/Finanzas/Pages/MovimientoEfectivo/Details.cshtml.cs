using System.Threading;
using System.Threading.Tasks;
using Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Dtos;
using Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.MovimientoEfectivo
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para details.
    /// </summary>
[AuthorizeMatvi(Funcion.Finanzas_Movimientos_Efectivo)]
    public class DetailsModel(
        IMediator _mediator
    ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa movimiento.
        /// </summary>
public DetallarMovimientoEfectivoDto Movimiento { get; set; } = default!;

                /// <inheritdoc/>
public async Task<IActionResult> OnGetAsync(string origen, string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(origen) || string.IsNullOrWhiteSpace(id))
                return NotFound();

            var idDoc = ToInt(id);
            var mov = await _mediator.Send(new DetallarMovimientoEfectivoQry(origen, idDoc), cancellationToken);

            if (mov == null) return NotFound();

            Movimiento = mov;
            return Page();
        }
    }
}
