using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Dtos;

namespace Matvi.Front.UI.Areas.Productos.Pages.PresentacionProducto.ViewModels
{
        /// <summary>
    /// Representa la clase para listar presentacion producto vm.
    /// </summary>
public class ListarPresentacionProductoVm
    {
                /// <summary>
        /// Identificador único de ct prod safe id.
        /// </summary>
public string CtProd_SafeId { get; set; } = string.Empty;
                /// <summary>
        /// Colección de elementos asociados a presentaciones producto.
        /// </summary>
public List<ListarProductoSucursalDto>? PresentacionesProducto { get; set; }

    }
}
