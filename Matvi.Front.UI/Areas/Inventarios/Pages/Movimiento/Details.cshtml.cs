using Matvi.Back.Application.Modules.Inventarios.Movimiento.Dtos;
using Matvi.Back.Application.Modules.Inventarios.Movimiento.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Inventarios.Pages.Movimiento
{
    [AuthorizeMatvi(Funcion.Inventarios_Movimientos)]
    public class DetailsModel(
        IMediator _mediator
    ) : BaseModel
    {
        public DetalleMovimientoDto? Movimiento { get; set; }

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