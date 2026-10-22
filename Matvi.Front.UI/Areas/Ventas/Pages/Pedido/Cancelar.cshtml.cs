using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.Pedido.Commands;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using Matvi.Back.Application.Modules.Ventas.Pedido.Queries;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Ventas.Pages.Pedido
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para cancelar.
    /// </summary>
[AuthorizeMatvi(Funcion.Ventas_Pedidos, Accion.Cancelar)]
    public class CancelarModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService,
        IUnitOfWork _unitOfWork
    ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa pedido.
        /// </summary>
public DetallarPedidoDto? Pedido { get; set; }

        /// <summary>Id de la caja abierta del usuario en turno, null si no hay corte activo.</summary>
        public int? IdCajaActiva { get; set; }

                /// <inheritdoc/>
public async Task OnGetAsync(
            string id,
            CancellationToken cancellationToken)
        {
            var idPedido = ToInt(id);

            Pedido = await _mediator.Send(
                new DetallarPedidoQry(idPedido, puedeVerSensibles: true),
                cancellationToken);

            // Resolver caja activa del usuario para posible devolución en caja
            var sesion = _usuarioActualService.UsuarioSesion;
            if (sesion?.SucursalSesion?.Id != null)
            {
                IdCajaActiva = _unitOfWork.Query<OpCorteCajaEtt>()
                    .Where(c =>
                        c.IdCtSucursal == sesion.SucursalSesion.Id.Value &&
                        c.IdCtUsuario == sesion.Id &&
                        c.FechaCierre == null)
                    .Select(c => (int?)c.Id)
                    .FirstOrDefault();
            }
        }

                /// <summary>
        /// Ejecuta la operación de on post cancelar async.
        /// </summary>
        /// <param name="cancelar">Parámetro cancelar.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnPostCancelarAsync(
            [FromBody] CancelarPedidoDto? cancelar,
            CancellationToken cancellationToken)
        {
            if (cancelar == null)
            {
                return new JsonResult(OperationResult<bool>.Error("No se recibieron datos válidos para procesar la cancelación."));
            }

            var idUsuario = _usuarioActualService.UsuarioSesion?.Id;
            if (!idUsuario.HasValue)
            {
                var claimId = User.FindFirst(Matvi.Front.UI.Constants.AuthConstants.UserSessionIdKey)?.Value;
                if (short.TryParse(claimId, out var idClaim))
                {
                    idUsuario = idClaim;
                }
            }

            if (!idUsuario.HasValue)
            {
                return new JsonResult(OperationResult<bool>.Error("La sesión del usuario no es válida o ha expirado."));
            }

            cancelar.CtUsua_Id = idUsuario.Value;

            // Asignar la sucursal del pedido para la búsqueda de la caja activa
            if (cancelar.CtSucu_Id == 0 && _usuarioActualService.UsuarioSesion?.SucursalSesion?.Id != null)
            {
                cancelar.CtSucu_Id = _usuarioActualService.UsuarioSesion.SucursalSesion.Id.Value;
            }

            var res = await _mediator.Send(
                new CancelarPedidoCmd(cancelar),
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
