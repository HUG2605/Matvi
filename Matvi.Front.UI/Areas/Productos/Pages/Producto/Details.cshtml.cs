using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.Producto.Commands;
using Matvi.Back.Application.Modules.Productos.Producto.Dtos;
using Matvi.Back.Application.Modules.Productos.Producto.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Productos.Pages.Producto
{
    [AuthorizeMatvi(Funcion.Productos_Productos)]
    public class DetailsModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ) : BaseModel
    {
        public DetallarProductoDto? Producto { get; set; }

        public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idProducto = ToInt(id);
            Producto = await _mediator.Send(
                new DetallarProductoQry(idProducto), 
                cancellationToken);
        }
        public async Task<JsonResult> OnGetDelete(
            string id, 
            CancellationToken cancellationToken)
        {
            if (!_usuarioActualService.TienePermiso(
                Funcion.Productos_Productos,
                Accion.Eliminar))
            {
                return new JsonResult(OperationResult<bool>.Error("Acceso denegado"));
            }

            var idProducto = ToInt(id);
            var res = await _mediator.Send(
                new EliminarProductoCmd(idProducto), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
