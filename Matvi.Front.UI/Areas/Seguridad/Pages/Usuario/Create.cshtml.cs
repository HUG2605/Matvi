using Matvi.Back.Application.Modules.Seguridad.Rol.Queries;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Commands;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Dtos;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Matvi.Front.UI.Areas.Seguridad.Pages.Usuario
{
    [AuthorizeMatvi(Funcion.Seguridad_Usuarios, Accion.Crear)]
    public class CreateModel(
        IMediator _mediator
        ) : BaseModel
    {
        [BindProperty]
        public required CrearUsuarioDto Usuario { get; set; }

        public SelectList RolesSelectList { get; set; } = default!;

        public async Task OnGetAsync(
            CancellationToken cancellationToken)
        {
            var usuarioTask = _mediator.Send(
                new CrearUsuarioQry(), 
                cancellationToken);
            var rolesTask = _mediator.Send(
                new ListarCatalogoRolesQry(), 
                cancellationToken);

            await Task.WhenAll(usuarioTask, rolesTask);

            Usuario = usuarioTask.Result;
            RolesSelectList = new SelectList(rolesTask.Result, "Id", "Nombre");
        }

        public async Task<JsonResult> OnPostCreateAsync(
            CancellationToken cancellationToken)
        {
            var res = await _mediator.Send(
                new CrearUsuarioCmd(Usuario), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
