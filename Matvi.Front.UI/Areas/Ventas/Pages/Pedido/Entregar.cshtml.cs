using Matvi.Back.Application.Modules.Ventas.Pedido.Commands;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using Matvi.Back.Application.Modules.Ventas.Pedido.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using Matvi.Back.Application.Modules.Ventas.MedioPago.Queries;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Front.UI.Areas.Ventas.Pages.Pedido
{
    [AuthorizeMatvi(Funcion.Ventas_Pedidos, Accion.Entregar)]
    public class EntregarModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService,
        IUnitOfWork _unitOfWork
        ) : BaseModel
    {
        public DetallarPedidoDto? Pedido { get; set; }
        public SelectList MedioPagoSelectList { get; set; } = default!;
        public SelectList CuentaBancariaSelectList { get; set; } = default!;

        public async Task OnGetAsync(
            string id,
            CancellationToken cancellationToken)
        {
            var idPedido = ToInt(id);

            Pedido = await _mediator.Send(
                new DetallarPedidoQry(idPedido, puedeVerSensibles: true),
                cancellationToken);

            var mediosPago = await _mediator.Send(new ListarMedioPagoSelectQry(), cancellationToken);
            MedioPagoSelectList = new SelectList(mediosPago, "Id", "Nombre");

            var cuentas = await _unitOfWork.Query<CtCuentaBancariaEtt>()
                .Where(c => c.Activa)
                .Select(c => new { c.Id, Nombre = $"{c.Banco} - {c.NumeroCuenta}" })
                .ToListAsync(cancellationToken);
            CuentaBancariaSelectList = new SelectList(cuentas, "Id", "Nombre");
        }

        public async Task<JsonResult> OnPostEntregarAsync(
            [FromBody] EntregarPedidoDto entrega,
            CancellationToken cancellationToken)
        {
            entrega.IdCtUsuarioRepartidor = _usuarioActualService.UsuarioSesion!.Id;

            var res = await _mediator.Send(
                new EntregarPedidoCmd(entrega),
                cancellationToken);

            return new JsonResult(res);
        }
    }
}