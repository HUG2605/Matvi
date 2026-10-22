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
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para edit.
    /// </summary>
[AuthorizeMatvi(Funcion.Ventas_Clientes, Accion.Editar)]
    public class EditModel(
        IMediator _mediator
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa cliente.
        /// </summary>
[BindProperty]
        public required EditarClienteDto Cliente { get; set; }

                /// <summary>
        /// Propiedad que representa regimen fiscal select list.
        /// </summary>
public SelectList RegimenFiscalSelectList { get; set; } = default!;

                /// <inheritdoc/>
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

                /// <summary>
        /// Ejecuta la operación de on post edit async.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnPostEditAsync(CancellationToken cancellationToken)
        {
            Cliente.Id = ToInt(Request.Form["Cliente.SafeId"]!);
            var res = await _mediator.Send(new EditarClienteCmd(Cliente), cancellationToken);
            return new JsonResult(res);
        }
    }
}
