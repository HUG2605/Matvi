using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.Cliente.Commands;
using Matvi.Back.Application.Modules.Ventas.Cliente.Dtos;
using Matvi.Back.Application.Modules.Ventas.Cliente.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Ventas.Pages.Cliente
{
    [AuthorizeMatvi(Funcion.Ventas_Clientes)]
    public class DetailsModel(
        IUsuarioActualService _usuarioActualService,
        IMediator _mediator
        ) : BaseModel
    {
        public DetallarClienteDto? Cliente { get; set; }

        public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idCliente = ToInt(id);
            Cliente = await _mediator.Send(
                new DetallarCienteQry(idCliente), 
                cancellationToken);
        }

        public async Task<JsonResult> OnGetDelete(
            string id, 
            CancellationToken cancellationToken)
        {
            if (!_usuarioActualService.TienePermiso(
                Funcion.Ventas_Clientes,
                Accion.Eliminar))
            {
                return new JsonResult(OperationResult<bool>.Error("Acceso denegado"));
            }

            var idCliente = ToInt(id);
            var res = await _mediator.Send(
                new EliminarClienteCmd(idCliente), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
