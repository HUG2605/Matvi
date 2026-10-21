using Matvi.Back.Application.Modules.Ventas.Cliente.Commands;
using Matvi.Back.Application.Modules.Ventas.Cliente.Dtos;
using Matvi.Back.Application.Modules.Ventas.Cliente.Queries;
using Matvi.Back.Application.Modules.Ventas.RegimenFiscal.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Matvi.Front.UI.Areas.Ventas.Pages.Cliente
{
    [AuthorizeMatvi(Funcion.Ventas_Clientes, Accion.Editar)]
    public class EditModel(
        IMediator _mediator
        ) : BaseModel
    {
        [BindProperty]
        public required EditarClienteDto Cliente { get; set; }

        public SelectList RegimenFiscalSelectList { get; set; } = default!;

        public async Task OnGet(string id, CancellationToken cancellationToken)
        {
            var idCliente = ToInt(id);

            var clienteTask = _mediator.Send(
                new EditarClienteQry(idCliente),
                cancellationToken);

            var regimenFiscalTask = _mediator.Send(
                new ListarRegimenFiscalSelectQry(),
                cancellationToken);

            await Task.WhenAll(clienteTask, regimenFiscalTask);

            Cliente = clienteTask.Result;

            RegimenFiscalSelectList = new SelectList(regimenFiscalTask.Result,"Id","Nombre");
        }

        public async Task<JsonResult> OnPostEditAsync(CancellationToken cancellationToken)
        {
            Cliente.Id = ToInt(Request.Form["Cliente.SafeId"]!);
            var res = await _mediator.Send(new EditarClienteCmd(Cliente), cancellationToken);
            return new JsonResult(res);
        }
    }
}
