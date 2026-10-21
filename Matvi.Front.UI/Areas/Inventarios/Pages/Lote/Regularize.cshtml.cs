using Matvi.Back.Application.Modules.Inventarios.Lote.Commands;
using Matvi.Back.Application.Modules.Inventarios.Lote.Dtos;
using Matvi.Back.Application.Modules.Inventarios.Lote.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Inventarios.Pages.Lote
{
    public class RegularizeModel(
        IMediator _mediator
        ) : BaseModel
    {
        [BindProperty]
        public required RegularizarLoteDto Lote { get; set; }

        public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idLote = ToInt(id);
            Lote = await _mediator.Send(
                new RegularizarLoteQry(idLote), 
                cancellationToken);
        }

        public async Task<JsonResult> OnPostRegularizeAsync(
            CancellationToken cancellationToken)
        {
            Lote.Id = ToInt(Request.Form["Lote.SafeId"]!);
            var res = await _mediator.Send(
                new RegularizarLoteCmd(Lote), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}