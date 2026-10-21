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
    [AuthorizeMatvi(Funcion.Finanzas_Movimientos_Bancarios)]
    public class DetailsModel(
        IMediator _mediator
    ) : BaseModel
    {
        public DetallarMovimientoBancarioDto Movimiento { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var idMov = ToInt(id);
            var mov = await _mediator.Send(new DetallarMovimientoBancarioQry(idMov), cancellationToken);

            if (mov == null) return NotFound();

            Movimiento = mov;
            return Page();
        }

        public async Task<JsonResult> OnPostAlternarConciliacionAsync(string safeId, CancellationToken cancellationToken)
        {
            var idMov = ToInt(safeId);
            var resultado = await _mediator.Send(new AlternarConciliacionCmd(idMov), cancellationToken);
            return new JsonResult(resultado);
        }
    }
}
