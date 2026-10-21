using Matvi.Back.Application.Modules.Compras.Proveedor.Commands;
using Matvi.Back.Application.Modules.Compras.Proveedor.Dtos;
using Matvi.Back.Application.Modules.Compras.Proveedor.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Compras.Pages.Proveedor
{
    [AuthorizeMatvi(Funcion.Compras_Proveedores, Accion.Editar)]
    public class EditModel(
        IMediator _mediator
        ) : BaseModel
    {
        [BindProperty]
        public required EditarProveedorDto Proveedor { get; set; }

        public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idProveedor = ToInt(id);
            Proveedor = await _mediator.Send(
                new EditarProveedorQry(idProveedor), 
                cancellationToken);
        }
        public async Task<JsonResult> OnPostEditAsync(
            CancellationToken cancellationToken)
        {
            Proveedor.Id = ToInt(Request.Form["Proveedor.SafeId"]!);
            var res = await _mediator.Send(
                new EditarProveedorCmd(Proveedor), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
