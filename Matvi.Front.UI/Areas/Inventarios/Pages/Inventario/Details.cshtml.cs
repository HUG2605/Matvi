using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Dtos;
using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;

namespace Matvi.Front.UI.Areas.Inventarios.Pages.Inventario
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para details.
    /// </summary>
[AuthorizeMatvi(Funcion.Inventarios_Inventario)]
    public class DetailsModel(    
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa producto sucursal.
        /// </summary>
public DetallarProductoSucursalDto? ProductoSucursal { get; set; }

                /// <inheritdoc/>
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
