using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Commands;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Dtos;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.TipoGasto
{
    [AuthorizeMatvi(Funcion.Finanzas_Tipos_de_Gastos, Accion.Editar)]
    public class EditModel(
        IMediator _mediator
        ) : BaseModel
    {
        [BindProperty]
        public required EditarTipoGastoDto TipoGasto { get; set; }

        public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idTipoGasto = ToByte(id);
            TipoGasto = await _mediator.Send(
                new EditarTipoGastoQry(idTipoGasto), 
                cancellationToken);
        }

        public async Task<JsonResult> OnPostEditAsync(
            CancellationToken cancellationToken)
        {
            TipoGasto.Id = ToByte(Request.Form["TipoGasto.SafeId"]!);
            var res = await _mediator.Send(
                new EditarTipoGastoCmd(TipoGasto), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
