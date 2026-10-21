using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Dtos;
using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;

namespace Matvi.Front.UI.Areas.Inventarios.Pages.Inventario
{
    [AuthorizeMatvi(Funcion.Inventarios_Inventario)]
    public class DetailsModel(    
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ) : BaseModel
    {
        public DetallarProductoSucursalDto? ProductoSucursal { get; set; }

        public async Task OnGet(
            string productoId,
            CancellationToken cancellationToken
            )
        {
            var idProducto = ToInt(productoId);

            ProductoSucursal = await _mediator.Send(
                new DetallarProductoSucursalQry(
                    idProducto,
                    _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value
                ), cancellationToken);
        }
    }
}
