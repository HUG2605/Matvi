using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Dtos;

namespace Matvi.Front.UI.Areas.Productos.Pages.PresentacionProducto.ViewModels
{
    public class ListarPresentacionProductoVm
    {
        public string CtProd_SafeId { get; set; } = string.Empty;
        public List<ListarProductoSucursalDto>? PresentacionesProducto { get; set; }

    }
}
