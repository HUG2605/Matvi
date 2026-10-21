using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Inventarios.Lote.Dtos
{
    public class RegularizarLoteDto : IIdDto<int>, IFolioDto, IProductoBaseDto, IPresentacionProductoBaseDto
    {
        #region Implementacion de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        #region Implementacion de IFolioDto

        public string CtSucu_Clave { get; set; } = string.Empty;
        public int Numero { get; set; }
        public string Folio => this.GetFolio(TipoConsecutivoEnm.COMPRA);

        #endregion

        #region implementacion de IProductoBaseDto

        public string CtProd_Nombre { get; set; } = string.Empty;
        public string? CtProd_EspecificacionTecnica { get; set; }
        public string? CtProd_Marca { get; set; }

        [Display(Name = "Producto")]
        public string CtProd_NombreCompleto => this.GetNombreCompleto();

        #endregion

        #region Implementacion de IPresentacionProductoBaseDto

        public int CtPresProd_IdCtProducto { get; set; }

        [Display(Name = "Presentación")]
        public string CtPresProd_Nombre { get; set; } = string.Empty;

        [Display(Name = "Factor UBI")]
        public decimal CtPresProd_FactorUBI { get; set; }

        #endregion

        [Display(Name = "Sucursal")]
        public string CtSucu_Nombre { get; set; } = string.Empty;

        [Display(Name = "Tipo de lote")]
        public string CtTipoLote_Nombre { get; set; } = string.Empty;

        [Display(Name = "Costo unitario por presentación")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public required decimal CostoUnitarioFld { get; set; }

        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        public string CtUnidBaseInve_Abreviatura { get; set; } = string.Empty;

        [Display(Name = "Existencia actual")]
        public decimal ExistenciaActualFld { get; set; }


        
    }
}
