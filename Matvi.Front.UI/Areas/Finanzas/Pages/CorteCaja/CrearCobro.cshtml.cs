using Matvi.Back.Application.Modules.Finanzas.Cobro.Commands;
using Matvi.Back.Application.Modules.Finanzas.Cobro.Dtos;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Queries;
using Matvi.Back.Application.Modules.Ventas.MedioPago.Queries;
using Matvi.Back.Application.Modules.Ventas.Pedido.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.CorteCaja
{
    [AuthorizeMatvi(Funcion.Finanzas_Cobros, Accion.Crear)]
    public class CrearCobroModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService,
        Matvi.Back.Infrastructure.Persistence.Repositories.IUnitOfWork _unitOfWork
    ) : BaseModel
    {
        [BindProperty]
        public required CrearCobroDto Cobro { get; set; }

        public SelectList MedioPagoSelectList { get; set; } = default!;

        public string? NombreClienteInicial { get; set; }
        public int? IdClienteInicial { get; set; }

        public async Task OnGetAsync(int? idCliente, CancellationToken cancellationToken)
        {
            var mediosPago = await _mediator.Send(new ListarMedioPagoSelectQry(), cancellationToken);
            MedioPagoSelectList = new SelectList(mediosPago, "Id", "Nombre");

            Cobro = new CrearCobroDto();

            if (idCliente.HasValue && idCliente.Value > 0)
            {
                IdClienteInicial = idCliente.Value;
                Cobro.CtClie_IdFld = idCliente.Value;

                var cliente = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(
                    _unitOfWork.Query<Matvi.Back.Infrastructure.Persistence.Entities.CtClienteEtt>(),
                    c => c.Id == idCliente.Value,
                    cancellationToken);

                NombreClienteInicial = cliente?.Nombre ?? $"Cliente #{idCliente.Value}";
            }

            if (_usuarioActualService.EstaSucursalEnSesionYRelacionada == true)
            {
                var cajaActiva = await _mediator.Send(new ObtenerCajaActivaQry(
                    _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value,
                    _usuarioActualService.UsuarioSesion!.Id), cancellationToken);

                Cobro.IdOpCorteCaja = cajaActiva?.Id ?? 0;
            }
        }

        public async Task<JsonResult> OnPostCreateAsync(CancellationToken cancellationToken)
        {
            Cobro.CtSucu_Id = _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value;
            Cobro.CtUsua_Id = _usuarioActualService.UsuarioSesion!.Id;

            var res = await _mediator.Send(new CrearCobroCmd(Cobro), cancellationToken);
            return new JsonResult(res);
        }

        public async Task<JsonResult> OnGetSaldoPendienteAsync(
            int idCliente,
            CancellationToken cancellationToken)
        {
            var saldo = await _mediator.Send(new ObtenerSaldoPendienteClienteQry(
                _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value,
                idCliente
            ), cancellationToken);

            return new JsonResult(saldo);
        }
    }
}