using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para crear presentacion producto.
    /// </summary>
public partial class CrearPresentacionProductoDto : IIdDto<int>, IProductoBaseDto
    {
        #region Implementacion de IIdDto

                /// <summary>
        /// Identificador único de id.
        /// </summary>
public int Id { get; set; }
                /// <summary>
        /// Identificador único de safe id.
        /// </summary>
public string SafeId => Id.GetSafeId();

        #endregion

        #region implementacion de IProductoBaseDto

                /// <summary>
        /// Texto descriptivo o detalle de ct prod nombre.
        /// </summary>
[Display(Name = "Producto")]
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

        //IdCtProducto    int	4	no
                /// <summary>
        /// Propiedad que representa ct prod id.
        /// </summary>
public required int CtProd_IdFld { get; set; }

        //Nombre    varchar	50	no
                /// <summary>
        /// Texto descriptivo o detalle de nombre.
        /// </summary>
[Display(Name = "Nombre de la presentación")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public required string NombreFld { get; set; }

        //FactorUBI    decimal	9	no
        /// <summary>
        /// Factor un unudades base de CtProducto
        /// </summary>
        [Display(Name = "Equivalencia en UBI's")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [Range(0.00001, double.MaxValue, ErrorMessage = "El valor para {0} debe ser mayor a 0.")]
        public required decimal FactorUBIFld { get; set; }

                /// <summary>
        /// Propiedad que representa ct unid base inve abreviatura.
        /// </summary>
public string CtUnidBaseInve_Abreviatura { get; set; } = string.Empty;

        //EsParaCompra    bit	1	no
                /// <summary>
        /// Indica si es para compra se encuentra activo o habilitado.
        /// </summary>
[Display(Name = "Disponible para compras")]
        public bool EsParaCompraFld { get; set; }

        //EsParaVenta bit	1	no
                /// <summary>
        /// Indica si es para venta se encuentra activo o habilitado.
        /// </summary>
[Display(Name = "Disponible para ventas")]
        public bool EsParaVentaFld { get; set; }

        //PrecioVenta decimal	9	no
                /// <summary>
        /// Monto económico correspondiente a precio venta.
        /// </summary>
[Display(Name = "Precio de venta")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [Range(0.00001, double.MaxValue, ErrorMessage = "El valor para {0} debe ser mayor a 0.")]
        public required decimal PrecioVentaFld { get; set; }

        //PrecioMayoreo   decimal	9	yes
                /// <summary>
        /// Monto económico correspondiente a precio mayoreo.
        /// </summary>
[Display(Name = "Precio de mayoreo")]
        [Range(0.00001, double.MaxValue, ErrorMessage = "El valor para {0} debe ser mayor a 0.")]
        public decimal? PrecioMayoreoFld { get; set; }

        //UnidadesMayoreo tinyint	1	yes
                /// <summary>
        /// Propiedad que representa unidades mayoreo.
        /// </summary>
[Display(Name = "Cantidad mínima para mayoreo")]
        [Range(1, double.MaxValue, ErrorMessage = "El valor para {0} debe ser mayor a 0.")]
        public short? UnidadesMayoreoFld { get; set; }

                /// <summary>
        /// Indica si es presentacion base se encuentra activo o habilitado.
        /// </summary>
public bool EsPresentacionBase { get; set; }

    }

}
