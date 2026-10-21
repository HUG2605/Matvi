using Matvi.Back.Application.Modules.Seguridad.Rol.Commands;
using Matvi.Back.Application.Modules.Seguridad.Rol.Dtos;
using Matvi.Back.Application.Modules.Seguridad.Rol.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Seguridad.Pages.Rol
{
    [AuthorizeMatvi(Funcion.Seguridad_Roles, Accion.Editar)]
    public class EditModel(
        IMediator _mediator
        ) : BaseModel
    {
        [BindProperty]
        public required EditarRolDto Rol { get; set; }

        public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idRol = ToByte(id);
            Rol = await _mediator.Send(
                new EditarRolQry(idRol), 
                cancellationToken);
        }

        public async Task<JsonResult> OnPostEditAsync(
            CancellationToken cancellationToken)
        {
            Rol.Id = ToShort(Request.Form["Rol.SafeId"]!);
            var res = await _mediator.Send(
                new EditarRolCmd(Rol), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
