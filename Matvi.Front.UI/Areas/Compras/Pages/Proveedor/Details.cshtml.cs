using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Compras.Proveedor.Commands;
using Matvi.Back.Application.Modules.Compras.Proveedor.Dtos;
using Matvi.Back.Application.Modules.Compras.Proveedor.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Compras.Pages.Proveedor
{
    [AuthorizeMatvi(Funcion.Compras_Proveedores)]
    public class DetailsModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService   
        ) : BaseModel
    {
        public DetallarProveedorDto? Proveedor { get; set; }

        public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idProveedor = ToInt(id);
            Proveedor = await _mediator.Send(
                new DetallarProveedorQry(idProveedor), 
                cancellationToken);
        }

        public async Task<JsonResult> OnGetDelete(
            string id, 
            CancellationToken cancellationToken)
        {
            if (!_usuarioActualService.TienePermiso(
                Funcion.Compras_Proveedores,
                Accion.Eliminar))
            {
                return new JsonResult(OperationResult<bool>.Error("Acceso denegado"));
            }

            var idProveedor = ToInt(id);
            var res = await _mediator.Send(
                new EliminarProveedorCmd(idProveedor), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
