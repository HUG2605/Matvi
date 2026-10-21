using Matvi.Back.Application.Modules.Ventas.Pedido.Commands;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using Matvi.Back.Application.Modules.Ventas.Pedido.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using Matvi.Front.UI.Security.Services;

namespace Matvi.Front.UI.Areas.Ventas.Pages.Pedido
{
    [AuthorizeMatvi(Funcion.Ventas_Pedidos, Accion.Surtir)]
    public class SupplyModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ) : BaseModel
    {
        public DetallarPedidoDto? Pedido { get; set; }

        public async Task OnGetAsync(
            string id,
            CancellationToken cancellationToken)
        {
            var idPedido = ToInt(id);

            Pedido = await _mediator.Send(
                new DetallarPedidoQry(idPedido, puedeVerSensibles: false),
                cancellationToken);
        }

        public async Task<JsonResult> OnPostSupplyAsync(
            [FromBody] SurtirPedidoDto surtir,
            CancellationToken cancellationToken)
        {
            surtir.CtUsua_Id = _usuarioActualService.UsuarioSesion!.Id;

            var res = await _mediator.Send(
                new SurtirPedidoCmd(surtir),
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
