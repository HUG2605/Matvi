using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.CanalCaptura.Queries;
using Matvi.Back.Application.Modules.Ventas.Cliente.Queries;
using Matvi.Back.Application.Modules.Ventas.MedioPago.Queries;
using Matvi.Back.Application.Modules.Ventas.Pedido.Commands;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using Matvi.Back.Application.Modules.Ventas.Pedido.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Extensions;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Matvi.Front.UI.Areas.Ventas.Pages.Pedido
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para create.
    /// </summary>
[AuthorizeMatvi(Funcion.Ventas_Pedidos, Accion.Crear)]
    public class CreateModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa pedido.
        /// </summary>
[BindProperty]
        public required CrearPedidoDto Pedido { get; set; }

        #region Generales 
        
                /// <summary>
        /// Propiedad que representa canal captura select list.
        /// </summary>
public SelectList CanalCapturaSelectList { get; set; } = default!;

                /// <summary>
        /// Propiedad que representa medio pago select list.
        /// </summary>
public SelectList MedioPagoSelectList { get; set; } = default!;

        #endregion

                /// <inheritdoc/>
public async Task OnGetAsync(
            CancellationToken cancellationToken)
        {
            // Limpiar sesión del pedido al entrar a la página
            HttpContext.Session.Remove("Pedido");

            var canalCapturaTask = _mediator.Send(
                new ListarCanalCapturaSelectQry(), 
                cancellationToken);
            var medioPagoTask = _mediator.Send(
                new ListarMedioPagoSelectQry(), 
                cancellationToken);

            await Task.WhenAll(canalCapturaTask, medioPagoTask);

            CanalCapturaSelectList = new SelectList(await canalCapturaTask, "Id", "Nombre");
            MedioPagoSelectList = new SelectList(await medioPagoTask, "Id", "Nombre");
        }

        #region Otras acciones

                /// <summary>
        /// Ejecuta la operación de on get estado credito cliente async.
        /// </summary>
        /// <param name="idCliente">Identificador único del registro u objeto.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnGetEstadoCreditoClienteAsync(
            int idCliente,
            CancellationToken cancellationToken)
        {
            var estadoCredito = await _mediator.Send(
                new ObtenerEstadoCreditoClienteQry(idCliente),
                cancellationToken);

            return new JsonResult(estadoCredito);
        }

                /// <summary>
        /// Ejecuta la operación de on get producto sucursal async.
        /// </summary>
        /// <param name="idProducto">Identificador único del registro u objeto.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnGetProductoSucursalAsync(
            int idProducto,
            CancellationToken cancellationToken)
        {
            var sucursalId = _usuarioActualService.SucursalId;
            if (!sucursalId.HasValue)
            {
                return new JsonResult(new ObtenerProductoSucursalDto
                {
                    RlProdSucu_Regularizado = false,
                    Regularizado = false,
                    CtProd_BajoPedido = false
                });
            }

            var esBajoPedido = await _mediator.Send(
                new ObtenerProductoSucursalQry(
                    sucursalId.Value,
                    idProducto), 
                cancellationToken);

            return new JsonResult(esBajoPedido);
        }

                /// <summary>
        /// Ejecuta la operación de on get presentacion producto async.
        /// </summary>
        /// <param name="idPresentacionProducto">Identificador único del registro u objeto.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnGetPresentacionProductoAsync(
            int idPresentacionProducto, 
            CancellationToken cancellationToken)
        {
            var sucursalId = _usuarioActualService.SucursalId;
            if (!sucursalId.HasValue)
            {
                return new JsonResult(new ObtenerPresentacionProductoDto
                {
                    ExistenciasPresentacion = 0,
                    RlProdSucu_Regularizado = false,
                    CtProd_BajoPedido = false
                });
            }

            var presProd = await _mediator.Send(
                new ObtenerPresentacionProductoQry(
                    sucursalId.Value,
                    idPresentacionProducto), 
                cancellationToken);

            return new JsonResult(presProd);
        }

        #endregion

        #region Productos del pedido

                /// <summary>
        /// Ejecuta la operación de on post agregar producto pedido async.
        /// </summary>
        /// <param name="idPresentacionProducto">Identificador único del registro u objeto.</param>
        /// <param name="cantidad">Parámetro cantidad.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<IActionResult> OnPostAgregarProductoPedidoAsync(
            int idPresentacionProducto,
            short cantidad,
            CancellationToken cancellationToken)
        {
            var sucursalId = _usuarioActualService.SucursalId;
            if (!sucursalId.HasValue)
            {
                return new JsonResult(OperationResult<AgregarProductoPedidoDto>.Error(
                    "No se ha seleccionado una sucursal activa en la sesión."));
            }

            // 1. Validar producto contra inventario
            var result = await _mediator.Send(
                new ValidarProductoPedidoQry(
                    sucursalId.Value,
                    idPresentacionProducto,
                    cantidad), 
                cancellationToken);

            // 2. Si no es válido → mostrar info
            if (result.Data == null)
            {
                return new JsonResult(result);
            }

            var dto = result.Data;

            // 3. Obtener lista de sesión
            var lista = HttpContext.Session.GetObject<List<AgregarProductoPedidoDto>>("Pedido");

            if (lista == null)
                lista = new List<AgregarProductoPedidoDto>();

            // 4. Si ya existe → eliminarlo (para recalcular todo)
            var existente = lista.FirstOrDefault(x => x.CtPresProd_IdFld == idPresentacionProducto);

            if (existente != null)
                lista.Remove(existente);

            lista.Insert(0, dto); // siempre al inicio

            // 5. Guardar en sesión
            HttpContext.Session.SetObject("Pedido", lista);

            // 6. Regresar partial actualizado
            return Partial("_ListAgregarProducto", lista);
        }

                /// <summary>
        /// Ejecuta la operación de on post eliminar producto pedido.
        /// </summary>
        /// <param name="idPresentacionProducto">Identificador único del registro u objeto.</param>
        /// <returns>Objeto con la información resultante de on post eliminar producto pedido.</returns>
public IActionResult OnPostEliminarProductoPedido(
            int idPresentacionProducto)
        {
            // 1. Obtener lista de sesión
            var lista = HttpContext.Session.GetObject<List<AgregarProductoPedidoDto>>("Pedido")
                        ?? new List<AgregarProductoPedidoDto>();

            // 2. Buscar y eliminar el producto
            var existente = lista.FirstOrDefault(x => x.CtPresProd_IdFld == idPresentacionProducto);

            if (existente != null)
                lista.Remove(existente);

            // 3. Guardar lista actualizada en sesión
            HttpContext.Session.SetObject("Pedido", lista);

            // 4. Regresar partial actualizado
            return Partial("_ListAgregarProducto", lista);
        }

                /// <summary>
        /// Ejecuta la operación de on post cambiar entregado mostrador.
        /// </summary>
        /// <param name="idPresentacionProducto">Identificador único del registro u objeto.</param>
        /// <param name="entregadoMostrador">Parámetro entregado mostrador.</param>
        /// <returns>Objeto con la información resultante de on post cambiar entregado mostrador.</returns>
public IActionResult OnPostCambiarEntregadoMostrador(
            int idPresentacionProducto,
            bool entregadoMostrador)
        {
            var lista = HttpContext.Session.GetObject<List<AgregarProductoPedidoDto>>("Pedido")
                        ?? new List<AgregarProductoPedidoDto>();

            var existente = lista.FirstOrDefault(x => x.CtPresProd_IdFld == idPresentacionProducto);

            if (existente != null)
                existente.EntregadoMostrador = entregadoMostrador;

            HttpContext.Session.SetObject("Pedido", lista);

            return new OkResult();
        }

        #endregion

                /// <summary>
        /// Ejecuta la operación de on post create async.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnPostCreateAsync(
            CancellationToken cancellationToken)
        {
            var sucursalId = _usuarioActualService.SucursalId;
            if (!sucursalId.HasValue)
            {
                return new JsonResult(OperationResult<int>.Error(
                    "No se ha seleccionado una sucursal activa en la sesión."));
            }

            var lista = HttpContext.Session.GetObject<List<AgregarProductoPedidoDto>>("Pedido");

            Pedido.Productos = lista;
            Pedido.CtSucu_Id = sucursalId.Value;
            Pedido.CtUsua_Id = _usuarioActualService.UsuarioSesion!.Id;

            var res = await _mediator.Send(
                new CrearPedidoCmd(Pedido), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
