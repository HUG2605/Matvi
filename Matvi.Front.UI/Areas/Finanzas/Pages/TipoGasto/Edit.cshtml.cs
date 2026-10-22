using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Commands;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Dtos;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.TipoGasto
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para edit.
    /// </summary>
[AuthorizeMatvi(Funcion.Finanzas_Tipos_de_Gastos, Accion.Editar)]
    public class EditModel(
        IMediator _mediator
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa tipo gasto.
        /// </summary>
[BindProperty]
        public required EditarTipoGastoDto TipoGasto { get; set; }

                /// <inheritdoc/>
public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idTipoGasto = ToByte(id);
            TipoGasto = await _mediator.Send(
                new EditarTipoGastoQry(idTipoGasto), 
                cancellationToken);
        }

                /// <summary>
        /// Ejecuta la operación de on post edit async.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
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
