using Matvi.Back.Application.Modules.Compras.Compra.Commands;
using Matvi.Back.Application.Modules.Compras.Compra.Dtos;
using Matvi.Back.Application.Modules.Compras.Compra.Queries;
using Matvi.Back.Application.Modules.Ventas.MedioPago.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Extensions;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Matvi.Front.UI.Areas.Compras.Pages.Compra
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para create.
    /// </summary>
[AuthorizeMatvi(Funcion.Compras_Compras, Accion.Crear)]
    public class CreateModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa compra.
        /// </summary>
[BindProperty]
        public required CrearCompraDto Compra { get; set; }

                /// <summary>
        /// Propiedad que representa medio pago select list.
        /// </summary>
public SelectList MedioPagoSelectList { get; set; } = default!;

                /// <inheritdoc/>
public async Task OnGetAsync(
            CancellationToken cancellationToken)
        {
            HttpContext.Session.Remove("Compra");

            var MedioPagoTask = _mediator.Send(new ListarMedioPagoSelectQry() , cancellationToken);
            await Task.WhenAll(MedioPagoTask);

            MedioPagoSelectList = new SelectList(await MedioPagoTask, "Id", "Nombre");
        }

        #region Productos de la compra

                /// <summary>
        /// Ejecuta la operación de on post agregar producto compra async.
        /// </summary>
        /// <param name="idPresentacionProducto">Identificador único del registro u objeto.</param>
        /// <param name="cantidad">Parámetro cantidad.</param>
        /// <param name="costo">Parámetro costo.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<IActionResult> OnPostAgregarProductoCompraAsync(
            int idPresentacionProducto,
            decimal cantidad,
            decimal costo,
            CancellationToken cancellationToken)
        {
            var sucursalId = _usuarioActualService.UsuarioSesion?.SucursalSesion?.Id ?? 0;

            var result = await _mediator.Send(new ObtenerProductoParaCompraQry(
                idPresentacionProducto,
                cantidad,
                costo,
                sucursalId
            ), cancellationToken);

            if (result.Data == null)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = result.Message ?? "El producto no está regularizado en el inventario.",
                    messageType = "info"
                });
            }

            var lista = HttpContext.Session.GetObject<List<AgregarProductoCompraDto>>("Compra")
                        ?? new List<AgregarProductoCompraDto>();

            var existente = lista.FirstOrDefault(x => x.CtPresProd_IdFld == idPresentacionProducto);
            if (existente != null)
                lista.Remove(existente);

            lista.Insert(0, result.Data);

            HttpContext.Session.SetObject("Compra", lista);

            return Partial("_ListAgregarProductoCompra", lista);
        }

                /// <summary>
        /// Ejecuta la operación de on post eliminar producto compra.
        /// </summary>
        /// <param name="idPresentacionProducto">Identificador único del registro u objeto.</param>
        /// <returns>Objeto con la información resultante de on post eliminar producto compra.</returns>
public IActionResult OnPostEliminarProductoCompra(
            int idPresentacionProducto)
        {
            var lista = HttpContext.Session.GetObject<List<AgregarProductoCompraDto>>("Compra")
                        ?? new List<AgregarProductoCompraDto>();

            var existente = lista.FirstOrDefault(x => x.CtPresProd_IdFld == idPresentacionProducto);
            if (existente != null)
                lista.Remove(existente);

            HttpContext.Session.SetObject("Compra", lista);

            return Partial("_ListAgregarProductoCompra", lista);
        }

        #endregion

                /// <summary>
        /// Ejecuta la operación de on post create async.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnPostCreateAsync(CancellationToken cancellationToken)
        {
            var lista = HttpContext.Session.GetObject<List<AgregarProductoCompraDto>>("Compra");

            Compra.Productos = lista;
            Compra.CtSucu_Id = _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value;
            Compra.CtUsua_Id = _usuarioActualService.UsuarioSesion!.Id;


            var res = await _mediator.Send(new CrearCompraCmd(Compra), cancellationToken);
            return new JsonResult(res);
        }

                /// <summary>
        /// Ejecuta la operación de on get saldo vencido async.
        /// </summary>
        /// <param name="idProveedor">Identificador único del registro u objeto.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
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