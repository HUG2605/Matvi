using Matvi.Back.Application.Modules.Configuracion.Sucursal.Commands;
using Matvi.Back.Application.Modules.Configuracion.Sucursal.Dtos;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Configuracion.Pages.Sucursal
{
    [AuthorizeMatvi(Funcion.Configuracion_Sucursales, Accion.Crear)]
    public class CreateModel(
        IMediator _mediator
        ) : BaseModel
    {
        [BindProperty]
        public required CrearSucursalDto Sucursal { get; set; }

        public void OnGet()
        {
        }

        public async Task<JsonResult> OnPostCreateAsync(
            CancellationToken cancellationToken)
        {
            var res = await _mediator.Send(
                new CrearSucursalCmd(Sucursal), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
