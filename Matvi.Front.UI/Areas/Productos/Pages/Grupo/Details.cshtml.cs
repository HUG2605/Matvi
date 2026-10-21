using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.Grupo.Commands;
using Matvi.Back.Application.Modules.Productos.Grupo.Dtos;
using Matvi.Back.Application.Modules.Productos.Grupo.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Productos.Pages.Grupo
{
    [AuthorizeMatvi(Funcion.Productos_Grupos)]
    public class DetailsModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ) : BaseModel
    {
        public DetallarGrupoDto? Grupo { get; set; }

        public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idGrupo = ToByte(id);
            Grupo = await _mediator.Send(
                new DetallarGrupoQry(idGrupo), 
                cancellationToken);
        }
        public async Task<JsonResult> OnGetDelete(
            string id, 
            CancellationToken cancellationToken)
        {
            if (!_usuarioActualService.TienePermiso(
                Funcion.Productos_Grupos,
                Accion.Eliminar))
            {
                return new JsonResult(OperationResult<bool>.Error("Acceso denegado"));
            }

            var idGrupo = ToByte(id);
            var res = await _mediator.Send(
                new EliminarGrupoCmd(idGrupo), 
                cancellationToken);
            return new JsonResult(res);
        }
    }
}
