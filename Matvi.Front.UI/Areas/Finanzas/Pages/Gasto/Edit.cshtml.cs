using Matvi.Back.Application.Modules.Finanzas.Gasto.Commands;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Dtos;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Queries;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Queries;
using Matvi.Back.Application.Modules.Ventas.MedioPago.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.Gasto
{
    [AuthorizeMatvi(Funcion.Finanzas_Gastos, Accion.Editar)]
    public class EditModel(
        IMediator _mediator
        ) : BaseModel
    {
        [BindProperty]
        public required EditarGastoDto Gasto { get; set; }

        public SelectList TipoGastoSelectList { get; set; } = default!;
        public SelectList MedioPagoSelectList { get; set; } = default!;

        public async Task OnGet(string id, CancellationToken cancellationToken)
        {
            var idGasto = ToInt(id);

            var gastoTask = _mediator.Send(new EditarGastoQry(idGasto), cancellationToken);
            var tipoGastoTask = _mediator.Send(new ListarTipoGastoSelectQry(), cancellationToken);
            var medioPagoTask = _mediator.Send(new ListarMedioPagoSelectQry(), cancellationToken);

            await Task.WhenAll(gastoTask, tipoGastoTask, medioPagoTask);

            Gasto = gastoTask.Result;
            TipoGastoSelectList = new SelectList(tipoGastoTask.Result, "Id", "Nombre");
            MedioPagoSelectList = new SelectList(medioPagoTask.Result, "Id", "Nombre");
        }

        public async Task<JsonResult> OnPostEditAsync(CancellationToken cancellationToken)
        {
            Gasto.Id = ToInt(Request.Form["Gasto.SafeId"]!);
            var res = await _mediator.Send(new EditarGastoCmd(Gasto), cancellationToken);
            return new JsonResult(res);
        }
    }
}