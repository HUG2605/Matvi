using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Dtos
{
    public class EditarProductoSucursalDto : IProductoBaseDto
    {
        // Sucursal
        public byte CtSuc_Id { get; set; }

        public string CtSuc_Nombre { get; set; } = string.Empty;

        // Producto
        public int CtProd_Id { get; set; }

        public string CtProd_SafeId => CtProd_Id.ToString()?.ToObfuscated() ?? string.Empty;

        #region implementacion de IProductoBaseDto

        public string CtProd_Nombre { get; set; } = string.Empty;
        public string? CtProd_EspecificacionTecnica { get; set; }
        public string? CtProd_Marca { get; set; }
        public string CtProd_NombreCompleto => this.GetNombreCompleto();

        #endregion

        public string CtUnidBaseInve_Abreviatura { get; set; } = string.Empty;

        // Relación Producto en sucursal

        [Display(Name = "Stock mínimo en UBI")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor para {0} debe ser un número entero positivo.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El valor para {0} debe ser un número entero sin decimales.")]
        public decimal StockMinimoUBIFld { get; set; }

        [Display(Name = "Stock máximo en UBI")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor para {0} debe ser un número entero positivo.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El valor para {0} debe ser un número entero sin decimales.")]
        public decimal StockMaximoUBIFld { get; set; }

        public bool Regularizado { get; set; }

        [Display(Name = "Ubicación")]
        public string? UbicacionFld { get; set; }

    }
}
