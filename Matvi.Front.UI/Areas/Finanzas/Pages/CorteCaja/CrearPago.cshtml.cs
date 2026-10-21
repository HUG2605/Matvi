using Matvi.Back.Application.Modules.Compras.Compra.Queries;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Queries;
using Matvi.Back.Application.Modules.Finanzas.Pago.Commands;
using Matvi.Back.Application.Modules.Finanzas.Pago.Dtos;
using Matvi.Back.Application.Modules.Ventas.MedioPago.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.CorteCaja
{
    [AuthorizeMatvi(Funcion.Finanzas_Pagos, Accion.Crear)]
    public class CrearPagoModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
    ) : BaseModel
    {
        [BindProperty]
        public required CrearPagoDto Pago { get; set; }

        public SelectList MedioPagoSelectList { get; set; } = default!;

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            var medioPagoTask = _mediator.Send(new ListarMedioPagoSelectQry(), cancellationToken);
            await Task.WhenAll(medioPagoTask);

            MedioPagoSelectList = new SelectList(await medioPagoTask, "Id", "Nombre");

            Pago = new CrearPagoDto();

            if (_usuarioActualService.EstaSucursalEnSesionYRelacionada == true)
            {
                var cajaActiva = await _mediator.Send(new ObtenerCajaActivaQry(
                    _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value,
                    _usuarioActualService.UsuarioSesion!.Id), cancellationToken);

                Pago.IdOpCorteCaja = cajaActiva?.Id ?? 0;
            }
        }

        public async Task<JsonResult> OnPostCreateAsync(CancellationToken cancellationToken)
        {
            Pago.CtSucu_Id = _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value;
            Pago.CtUsua_Id = _usuarioActualService.UsuarioSesion!.Id;

            var res = await _mediator.Send(new CrearPagoCmd(Pago), cancellationToken);
            return new JsonResult(res);
        }

        public async Task<JsonResult> OnGetSaldoVencidoAsync(
            int idProveedor,
            CancellationToken cancellationToken)
        {
            var saldo = await _mediator.Send(new ObtenerSaldoVencidoQry(
                _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value,
                idProveedor
            ), cancellationToken);

            return new JsonResult(saldo);
        }
    }
}