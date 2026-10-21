using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Commands;
using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Dtos;
using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Productos.Pages.PresentacionProducto
{
    [AuthorizeMatvi(Funcion.Productos_Productos, Accion.Editar_PresentacionProducto)]
    public class EditModel(
        IMediator _mediator
        ) : BaseModel
    {
        [BindProperty]
        public required EditarPresentacionProductoDto PresentacionProducto { get; set; }

        public async Task OnGetAsync(
            string id, 
            CancellationToken cancellationToken)
        {
            var idPresentacionProducto = ToInt(id);
            PresentacionProducto = await _mediator.Send(
                new EditarPresentacionProductoQry(idPresentacionProducto), 
                cancellationToken);
        }

        public async Task<JsonResult> OnPostEditAsync(
            CancellationToken cancellationToken)
        {
            PresentacionProducto.Id = ToInt(Request.Form["PresentacionProducto.SafeId"]!);
            var res = await _mediator.Send(
                new EditarPresentacionProductoCmd(PresentacionProducto), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
