using System.Threading;
using System.Threading.Tasks;
using Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Commands;
using Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Dtos;
using Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.MovimientoBancario
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para details.
    /// </summary>
[AuthorizeMatvi(Funcion.Finanzas_Movimientos_Bancarios)]
    public class DetailsModel(
        IMediator _mediator
    ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa movimiento.
        /// </summary>
public DetallarMovimientoBancarioDto Movimiento { get; set; } = default!;

                /// <inheritdoc/>
public async Task<IActionResult> OnGetAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var idMov = ToInt(id);
            var mov = await _mediator.Send(new DetallarMovimientoBancarioQry(idMov), cancellationToken);

            if (mov == null) return NotFound();

            Movimiento = mov;
            return Page();
        }

                /// <summary>
        /// Ejecuta la operación de on post alternar conciliacion async.
        /// </summary>
        /// <param name="safeId">Parámetro safe id.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnPostAlternarConciliacionAsync(string safeId, CancellationToken cancellationToken)
        {
            var idMov = ToInt(safeId);
            var resultado = await _mediator.Send(new AlternarConciliacionCmd(idMov), cancellationToken);
            return new JsonResult(resultado);
        }
    }
}
