using Matvi.Back.Application.Modules.Productos.Grupo.Commands;
using Matvi.Back.Application.Modules.Productos.Grupo.Dtos;
using Matvi.Back.Application.Modules.Productos.Grupo.Queries;
using Matvi.Back.Application.Modules.Productos.Seccion.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Matvi.Front.UI.Areas.Productos.Pages.Grupo
{
    [AuthorizeMatvi(Funcion.Productos_Grupos, Accion.Editar)]
    public class EditModel(
        IMediator _mediator
        ) : BaseModel
    {
        [BindProperty]
        public required EditarGrupoDto Grupo { get; set; }

        public SelectList? SeccionSelectList { get; set; }
        
        public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idGrupo = ToByte(id);

            var grupoTask = _mediator.Send(
                new EditarGrupoQry(idGrupo), 
                cancellationToken);
            var seccionesTask = _mediator.Send(
                new ListarSeccionSelectQry(), 
                cancellationToken);

            await Task.WhenAll(grupoTask, seccionesTask);

            Grupo = grupoTask.Result;
            SeccionSelectList = new SelectList(seccionesTask.Result,"Id","Nombre");
        }

        public async Task<JsonResult> OnPostEditAsync(
            CancellationToken cancellationToken)
        {
            Grupo.Id = ToByte(Request.Form["Grupo.SafeId"]!);
            var res = await _mediator.Send(
                new EditarGrupoCmd(Grupo), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
