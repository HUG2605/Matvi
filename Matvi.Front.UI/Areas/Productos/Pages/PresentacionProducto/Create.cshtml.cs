using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Commands;
using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Dtos;
using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Productos.Pages.PresentacionProducto
{
    [AuthorizeMatvi(Funcion.Productos_Productos, Accion.Crear_PresentacionProducto)]
    public class CreateModel(
        IMediator _mediator
        ) : BaseModel
    {
        [BindProperty]
        public required CrearPresentacionProductoDto PresentacionProducto { get; set; }

        public async Task OnGetAsync(
            string productoId, 
            CancellationToken cancellationToken)
        {
            var idProducto = ToInt(productoId);
            PresentacionProducto = await _mediator.Send(
                new CrearPresentacionProductoQry(idProducto), 
                cancellationToken);
        }

        public async Task<JsonResult> OnPostCreateAsync(
            CancellationToken cancellationToken)
        {
            var res = await _mediator.Send(
                new CrearPresentacionProductoCmd(PresentacionProducto), 
                cancellationToken);
            return new JsonResult(res);
        }
    }
}
