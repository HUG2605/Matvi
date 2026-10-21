using Matvi.Back.Application.Modules.Productos.Seccion.Commands;
using Matvi.Back.Application.Modules.Productos.Seccion.Dtos;
using Matvi.Back.Application.Modules.Productos.Seccion.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Productos.Pages.Seccion
{
    [AuthorizeMatvi(Funcion.Productos_Secciones, Accion.Editar)]
    public class EditModel(
        IMediator _mediator
        ) : BaseModel
    {
        [BindProperty]
        public required EditarSeccionDto Seccion { get; set; }

        public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idSeccion = ToByte(id);
            Seccion = await _mediator.Send(
                new EditarSeccionQry(idSeccion), 
                cancellationToken);
        }

        public async Task<JsonResult> OnPostEditAsync(
            CancellationToken cancellationToken)
        {
            Seccion.Id = ToByte(Request.Form["Seccion.SafeId"]!);
            var res = await _mediator.Send(
                new EditarSeccionCmd(Seccion), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
