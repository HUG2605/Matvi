using Matvi.Back.Application.Modules.Productos.Seccion.Commands;
using Matvi.Back.Application.Modules.Productos.Seccion.Dtos;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Productos.Pages.Seccion
{
    [AuthorizeMatvi(Funcion.Productos_Secciones, Accion.Crear)]
    public class CreateModel(
        IMediator _mediator
        ) : BaseModel
    {
        [BindProperty]
        public required CrearSeccionDto Seccion { get; set; }

        public void OnGet()
        {
        }

        public async Task<JsonResult> OnPostCreateAsync(
            CancellationToken cancellationToken)
        {
            var res = await _mediator.Send(
                new CrearSeccionCmd(Seccion), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}