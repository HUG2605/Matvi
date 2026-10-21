using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using Matvi.Back.Application.Modules.Ventas.Pedido.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;

namespace Matvi.Front.UI.Areas.Ventas.Pages.Pedido
{
    [AuthorizeMatvi(Funcion.Ventas_Pedidos)]
    public class DetailsModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
    ) : BaseModel
    {
        public DetallarPedidoDto? Pedido { get; set; }

        public async Task OnGetAsync(
            string id,
            CancellationToken cancellationToken)
        {
            var puedeVerSensibles = _usuarioActualService
            .PuedeVerSensibles(Funcion.Ventas_Pedidos);
            var idPedido = ToInt(id);
            Pedido = await _mediator.Send(
                new DetallarPedidoQry(idPedido,puedeVerSensibles),
                cancellationToken);
        }
    }
}