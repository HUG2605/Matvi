using Matvi.Back.Application.Modules.Compras.Proveedor.Commands;
using Matvi.Back.Application.Modules.Compras.Proveedor.Dtos;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Compras.Pages.Proveedor
{
    [AuthorizeMatvi(Funcion.Compras_Proveedores , Accion.Crear)]
    public class CreateModel(
        IMediator _mediator
        ) : BaseModel
    {
        [BindProperty]
        public required CrearProveedorDto Proveedor { get; set; }

        public void OnGet()
        {
        }

        public async Task<JsonResult> OnPostCreateAsync(
            CancellationToken cancellationToken)
        {
            var res = await _mediator.Send(
                new CrearProveedorCmd(Proveedor), 
                cancellationToken);
            
            return new JsonResult(res);
        }
    }
}
