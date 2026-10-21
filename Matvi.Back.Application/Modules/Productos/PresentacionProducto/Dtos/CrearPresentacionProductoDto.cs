using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Dtos
{
    public partial class CrearPresentacionProductoDto : IIdDto<int>, IProductoBaseDto
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

        //IdCtProducto    int	4	no
        public required int CtProd_IdFld { get; set; }

        //Nombre    varchar	50	no
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

        public string CtUnidBaseInve_Abreviatura { get; set; } = string.Empty;

        //EsParaCompra    bit	1	no
        [Display(Name = "Disponible para compras")]
        public bool EsParaCompraFld { get; set; }

        //EsParaVenta bit	1	no
        [Display(Name = "Disponible para ventas")]
        public bool EsParaVentaFld { get; set; }

        //PrecioVenta decimal	9	no
        [Display(Name = "Precio de venta")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [Range(0.00001, double.MaxValue, ErrorMessage = "El valor para {0} debe ser mayor a 0.")]
        public required decimal PrecioVentaFld { get; set; }

        //PrecioMayoreo   decimal	9	yes
        [Display(Name = "Precio de mayoreo")]
        [Range(0.00001, double.MaxValue, ErrorMessage = "El valor para {0} debe ser mayor a 0.")]
        public decimal? PrecioMayoreoFld { get; set; }

        //UnidadesMayoreo tinyint	1	yes
        [Display(Name = "Cantidad mínima para mayoreo")]
        [Range(1, double.MaxValue, ErrorMessage = "El valor para {0} debe ser mayor a 0.")]
        public short? UnidadesMayoreoFld { get; set; }

        public bool EsPresentacionBase { get; set; }

    }

}
