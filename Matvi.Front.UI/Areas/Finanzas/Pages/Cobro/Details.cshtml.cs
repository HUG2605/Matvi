using Matvi.Back.Application.Modules.Finanzas.Cobro.Dtos;
using Matvi.Back.Application.Modules.Finanzas.Cobro.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.Cobro
{
    [AuthorizeMatvi(Funcion.Finanzas_Cobros)]
    public class DetailsModel(
        IMediator _mediator
    ) : BaseModel
    {
        public DetallarCobroDto? Cobro { get; set; }

        public async Task<IActionResult> OnGet(
            string id,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToPage("Index");

            var idCobro = ToInt(id);
            Cobro = await _mediator.Send(
                new DetallarCobroQry(idCobro),
                cancellationToken);

            if (Cobro == null)
                return NotFound();

            return Page();
        }
    }
}