 using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Productos.Producto.Dtos
{
    public class DetallarProductoDto : IIdDto<int>, IProductoBaseDto
    {
        #region Implementacion de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        #region implementacion de IProductoBaseDto

        [Display(Name = "Producto")]
        public string CtProd_Nombre { get; set; } = string.Empty;
        public string? CtProd_EspecificacionTecnica { get; set; }
        public string? CtProd_Marca { get; set; }
        public string CtProd_NombreCompleto => this.GetNombreCompleto();

        #endregion

        public string CtSecc_Nombre { get; set; } = string.Empty;
        public string CtGrup_Nombre { get; set; } = string.Empty;
        public string CtGrup_Descripcion { get; set; } = string.Empty;
        public string CtUnidBaseInve_Nombre { get; set; } = string.Empty;
        public bool BajoPedido { get; set; }
        public bool EntregaMostrador { get; set; }
        public string? Imagen { get; set; }
        public bool CodigoBarrasInterno { get; set; }
        public string? CodigoBarras { get; set; }
        public bool Activo { get; set; }
        public List<RelacionarProveedorDto>? Proveedores { get; set; }
    }
}
