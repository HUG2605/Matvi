using Matvi.Back.Application.Modules.Finanzas.Pago.Dtos;
using Matvi.Back.Application.Modules.Finanzas.Pago.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.Pago
{
    [AuthorizeMatvi(Funcion.Finanzas_Pagos)]
    public class DetailsModel(
        IMediator _mediator
    ) : BaseModel
    {
        public DetallarPagoDto? Pago { get; set; }

        public async Task<IActionResult> OnGet(
            string id,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToPage("Index");

            var idPago = ToInt(id);
            Pago = await _mediator.Send(
                new DetallarPagoQry(idPago),
                cancellationToken);

            if (Pago == null)
                return NotFound();

            return Page();
        }
    }
}