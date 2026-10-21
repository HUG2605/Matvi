using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Commands;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Dtos;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.TipoGasto
{
    [AuthorizeMatvi(Funcion.Finanzas_Tipos_de_Gastos)]
    public class DetailsModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ) : BaseModel
    {
        public DetallarTipoGastoDto? TipoGasto { get; set; }

        public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idTipoGasto = ToByte(id);
            TipoGasto = await _mediator.Send(
                new DetallarTipoGastoQry(idTipoGasto), 
                cancellationToken);
        }

        public async Task<JsonResult> OnGetDelete(
            string id, 
            CancellationToken cancellationToken)
        {
            if (!_usuarioActualService.TienePermiso(
                Funcion.Finanzas_Tipos_de_Gastos,
                Accion.Eliminar))
            {
                return new JsonResult(OperationResult<bool>.Error("Acceso denegado"));
            }

            var idTipoGasto = ToByte(id);
            var res = await _mediator.Send(
                new EliminarTipoGastoCmd(idTipoGasto), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
