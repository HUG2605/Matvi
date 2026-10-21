using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Configuracion.Sucursal.Commands;
using Matvi.Back.Application.Modules.Configuracion.Sucursal.Dtos;
using Matvi.Back.Application.Modules.Configuracion.Sucursal.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Configuracion.Pages.Sucursal
{
    [AuthorizeMatvi(Funcion.Configuracion_Sucursales)]
    public class DetailsModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ) : BaseModel
    {

        public DetallarSucursalDto? Sucursal { get; set; }

        public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idSucursal = ToByte(id);
            Sucursal = await _mediator.Send(
                new DetallarSucursalQry(idSucursal), 
                cancellationToken);
        }

        public async Task<JsonResult> OnGetDelete(
            string id, 
            CancellationToken cancellationToken)
        {
            if (!_usuarioActualService.TienePermiso(
                Funcion.Configuracion_Sucursales,
                Accion.Eliminar))
            {
                return new JsonResult(OperationResult<bool>.Error("Acceso denegado"));
            }

            var idSucursal = ToByte(id);
            var res = await _mediator.Send(
                new EliminarSucursalCmd(idSucursal), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
