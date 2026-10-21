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
    [AuthorizeMatvi(Funcion.Ventas_Clientes, Accion.Crear)]
    public class CreateModel(
        IMediator _mediator
        ) : BaseModel
    {
        [BindProperty]
        public required CrearClienteDto Cliente { get; set; }

        public SelectList RegimenFiscalSelectList { get; set; } = default!;

        public async Task OnGetAsync(
            CancellationToken cancellationToken)
        {
           var regimenFiscalList = await _mediator.Send(
               new ListarRegimenFiscalSelectQry(), 
               cancellationToken);
           RegimenFiscalSelectList = new SelectList(regimenFiscalList, "Id", "Nombre");
        }

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
