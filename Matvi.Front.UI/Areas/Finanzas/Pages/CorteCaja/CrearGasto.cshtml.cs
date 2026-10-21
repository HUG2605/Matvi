// Create.cshtml.cs
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Queries;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Commands;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Dtos;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Queries;
using Matvi.Back.Application.Modules.Ventas.MedioPago.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.CorteCaja
{
    [AuthorizeMatvi(Funcion.Finanzas_Gastos, Accion.Crear)]
    public class CrearGastoModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
    ) : BaseModel
    {
        [BindProperty]
        public required CrearGastoDto Gasto { get; set; }

        public SelectList MedioPagoSelectList { get; set; } = default!;
        public SelectList TipoGastoSelectList { get; set; } = default!;

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            var medioPagoTask = _mediator.Send(new ListarMedioPagoSelectQry(), cancellationToken);
            var tipoGastoTask = _mediator.Send(new ListarTipoGastoSelectQry(), cancellationToken);

            await Task.WhenAll(medioPagoTask, tipoGastoTask);

            MedioPagoSelectList = new SelectList(await medioPagoTask, "Id", "Nombre");
            TipoGastoSelectList = new SelectList(await tipoGastoTask, "Id", "Nombre");

            Gasto = new CrearGastoDto();

            if (_usuarioActualService.EstaSucursalEnSesionYRelacionada == true)
            {
                var cajaActiva = await _mediator.Send(new ObtenerCajaActivaQry(
                    _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value,
                    _usuarioActualService.UsuarioSesion!.Id), cancellationToken);

                Gasto.IdOpCorteCaja = cajaActiva?.Id ?? 0;
            }
        }

        public async Task<JsonResult> OnPostCreateAsync(CancellationToken cancellationToken)
        {
            Gasto.CtSucu_Id = _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value;
            Gasto.CtUsua_Id = _usuarioActualService.UsuarioSesion!.Id;

            var res = await _mediator.Send(new CrearGastoCmd(Gasto), cancellationToken);
            return new JsonResult(res);
        }
    }
}