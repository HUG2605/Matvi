using Matvi.Back.Application.Modules.Inventarios.Lote.Commands;
using Matvi.Back.Application.Modules.Inventarios.Lote.Dtos;
using Matvi.Back.Application.Modules.Inventarios.Lote.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Inventarios.Pages.Lote
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para regularize.
    /// </summary>
public class RegularizeModel(
        IMediator _mediator
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa lote.
        /// </summary>
[BindProperty]
        public required RegularizarLoteDto Lote { get; set; }

                /// <inheritdoc/>
public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idLote = ToInt(id);
            Lote = await _mediator.Send(
                new RegularizarLoteQry(idLote), 
                cancellationToken);
        }

                /// <summary>
        /// Ejecuta la operación de on post regularize async.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
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