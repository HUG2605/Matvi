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
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para crear cobro.
    /// </summary>
[AuthorizeMatvi(Funcion.Finanzas_Cobros, Accion.Crear)]
    public class CrearCobroModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService,
        Matvi.Back.Infrastructure.Persistence.Repositories.IUnitOfWork _unitOfWork
    ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa cobro.
        /// </summary>
[BindProperty]
        public required CrearCobroDto Cobro { get; set; }

                /// <summary>
        /// Propiedad que representa medio pago select list.
        /// </summary>
public SelectList MedioPagoSelectList { get; set; } = default!;

                /// <summary>
        /// Texto descriptivo o detalle de nombre cliente inicial.
        /// </summary>
public string? NombreClienteInicial { get; set; }
                /// <summary>
        /// Identificador único de id cliente inicial.
        /// </summary>
public int? IdClienteInicial { get; set; }

                /// <inheritdoc/>
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

                /// <summary>
        /// Ejecuta la operación de on post create async.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnPostCreateAsync(CancellationToken cancellationToken)
        {
            Cobro.CtSucu_Id = _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value;
            Cobro.CtUsua_Id = _usuarioActualService.UsuarioSesion!.Id;

            var res = await _mediator.Send(new CrearCobroCmd(Cobro), cancellationToken);
            return new JsonResult(res);
        }

                /// <summary>
        /// Ejecuta la operación de on get saldo pendiente async.
        /// </summary>
        /// <param name="idCliente">Identificador único del registro u objeto.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
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