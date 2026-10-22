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
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para supply.
    /// </summary>
[AuthorizeMatvi(Funcion.Ventas_Pedidos, Accion.Surtir)]
    public class SupplyModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa pedido.
        /// </summary>
public DetallarPedidoDto? Pedido { get; set; }

                /// <inheritdoc/>
public async Task OnGetAsync(
            string id,
            CancellationToken cancellationToken)
        {
            var idPedido = ToInt(id);

            Pedido = await _mediator.Send(
                new DetallarPedidoQry(idPedido, puedeVerSensibles: false),
                cancellationToken);
        }

                /// <summary>
        /// Ejecuta la operación de on post supply async.
        /// </summary>
        /// <param name="surtir">Parámetro surtir.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
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
