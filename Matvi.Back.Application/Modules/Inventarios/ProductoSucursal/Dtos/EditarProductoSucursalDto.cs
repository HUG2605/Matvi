using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para editar producto sucursal.
    /// </summary>
public class EditarProductoSucursalDto : IProductoBaseDto
    {
        // Sucursal
                /// <summary>
        /// Identificador único de ct suc id.
        /// </summary>
public byte CtSuc_Id { get; set; }

                /// <summary>
        /// Texto descriptivo o detalle de ct suc nombre.
        /// </summary>
public string CtSuc_Nombre { get; set; } = string.Empty;

        // Producto
                /// <summary>
        /// Identificador único de ct prod id.
        /// </summary>
public int CtProd_Id { get; set; }

                /// <summary>
        /// Identificador único de ct prod safe id.
        /// </summary>
public string CtProd_SafeId => CtProd_Id.ToString()?.ToObfuscated() ?? string.Empty;

        #region implementacion de IProductoBaseDto

                /// <summary>
        /// Texto descriptivo o detalle de ct prod nombre.
        /// </summary>
public string CtProd_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa ct prod especificacion tecnica.
        /// </summary>
public string? CtProd_EspecificacionTecnica { get; set; }
                /// <summary>
        /// Propiedad que representa ct prod marca.
        /// </summary>
public string? CtProd_Marca { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de ct prod nombre completo.
        /// </summary>
public string CtProd_NombreCompleto => this.GetNombreCompleto();

        #endregion

                /// <summary>
        /// Propiedad que representa ct unid base inve abreviatura.
        /// </summary>
public string CtUnidBaseInve_Abreviatura { get; set; } = string.Empty;

        // Relación Producto en sucursal

                /// <summary>
        /// Cantidad o volumen de unidades de stock minimo ubi.
        /// </summary>
[Display(Name = "Stock mínimo en UBI")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor para {0} debe ser un número entero positivo.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El valor para {0} debe ser un número entero sin decimales.")]
        public decimal StockMinimoUBIFld { get; set; }

                /// <summary>
        /// Cantidad o volumen de unidades de stock maximo ubi.
        /// </summary>
[Display(Name = "Stock máximo en UBI")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor para {0} debe ser un número entero positivo.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El valor para {0} debe ser un número entero sin decimales.")]
        public decimal StockMaximoUBIFld { get; set; }

                /// <summary>
        /// Propiedad que representa regularizado.
        /// </summary>
public bool Regularizado { get; set; }

                /// <summary>
        /// Propiedad que representa ubicacion.
        /// </summary>
[Display(Name = "Ubicación")]
        public string? UbicacionFld { get; set; }

    }
}
