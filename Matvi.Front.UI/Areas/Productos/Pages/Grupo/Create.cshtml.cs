using Matvi.Back.Application.Modules.Productos.Grupo.Commands;
using Matvi.Back.Application.Modules.Productos.Grupo.Dtos;
using Matvi.Back.Application.Modules.Productos.Seccion.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Matvi.Front.UI.Areas.Productos.Pages.Grupo
{
    [AuthorizeMatvi(Funcion.Productos_Grupos, Accion.Crear)]
    public class CreateModel(
        IMediator _mediator
        ) : BaseModel
    {
        [BindProperty]
        public required CrearGrupoDto Grupo { get; set; }

        public SelectList SeccionSelectList { get; set; } = default!;

        public async Task OnGetAsync(
            CancellationToken cancellationToken)
        {
            var seccionList = await _mediator.Send(
                new ListarSeccionSelectQry(), 
                cancellationToken);

            SeccionSelectList = new SelectList(seccionList, "Id", "Nombre");
        }

        public async Task<JsonResult> OnPostCreateAsync(
            CancellationToken cancellationToken)
        {
            var res = await _mediator.Send(
                new CrearGrupoCmd(Grupo), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
