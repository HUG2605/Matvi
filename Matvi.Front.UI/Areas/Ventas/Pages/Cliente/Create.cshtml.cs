using Matvi.Back.Application.Modules.Ventas.Cliente.Commands;
using Matvi.Back.Application.Modules.Ventas.Cliente.Dtos;
using Matvi.Back.Application.Modules.Ventas.RegimenFiscal.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Matvi.Front.UI.Areas.Ventas.Pages.Cliente
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para create.
    /// </summary>
[AuthorizeMatvi(Funcion.Ventas_Clientes, Accion.Crear)]
    public class CreateModel(
        IMediator _mediator
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa cliente.
        /// </summary>
[BindProperty]
        public required CrearClienteDto Cliente { get; set; }

                /// <summary>
        /// Propiedad que representa regimen fiscal select list.
        /// </summary>
public SelectList RegimenFiscalSelectList { get; set; } = default!;

                /// <inheritdoc/>
public async Task OnGetAsync(
            CancellationToken cancellationToken)
        {
           var regimenFiscalList = await _mediator.Send(
               new ListarRegimenFiscalSelectQry(), 
               cancellationToken);
           RegimenFiscalSelectList = new SelectList(regimenFiscalList, "Id", "Nombre");
        }

                /// <summary>
        /// Ejecuta la operación de on post create async.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnPostCreateAsync(
            CancellationToken cancellationToken)
        {
            var res = await _mediator.Send(
                new CrearClienteCmd(Cliente), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
