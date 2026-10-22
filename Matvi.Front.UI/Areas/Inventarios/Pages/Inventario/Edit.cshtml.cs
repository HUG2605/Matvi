using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Commands;
using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Dtos;
using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Inventarios.Pages.Inventario
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para edit.
    /// </summary>
[AuthorizeMatvi(Funcion.Inventarios_Inventario, Accion.Editar)]
    public class EditModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa producto sucursal.
        /// </summary>
[BindProperty]
        public required EditarProductoSucursalDto ProductoSucursal { get; set; }

                /// <inheritdoc/>
public async Task OnGet(string idProducto, CancellationToken cancellationToken)
        {
            var idProd = ToShort(idProducto);

            ProductoSucursal = await _mediator.Send(new EditarProductoSucursalQry(
                 idProd,
                _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value
                ), cancellationToken);
        }

                /// <summary>
        /// Ejecuta la operación de on post edit async.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnPostEditAsync(CancellationToken cancellationToken)
        {
            ProductoSucursal.CtProd_Id = ToShort(Request.Form["ProductoSucursal.CtProd_SafeId"]!);
            ProductoSucursal.CtSuc_Id = _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value;
            var res = await _mediator.Send(
                new EditarProductoSucursalCmd(ProductoSucursal), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
