using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Dtos
{
    public class ListarProductoSucursalDto : IProductoBaseDto
    {
        public int CtProd_Id { get; set; }

        public string CtProd_SafeId => CtProd_Id.ToString()?.ToObfuscated() ?? string.Empty;

        
        #region implementacion de IProductoBaseDto

        public string CtProd_Nombre { get; set; } = string.Empty;
        public string? CtProd_EspecificacionTecnica { get; set; }
        public string? CtProd_Marca { get; set; }
        public string CtProd_NombreCompleto => this.GetNombreCompleto();

        #endregion

        public string CtGrup_Nombre { get; set; } = string.Empty;

        public string CtSecc_Nombre { get; set; } = string.Empty;

        public string CtUnidBaseInve_Abreviatura { get; set; } = string.Empty;

        public decimal StockMinimoUBI { get; set; }

        public decimal ExistenciasUBI { get; set; }

        public decimal StockMaximoUBI { get; set; }

        public bool Regularizado { get; set; }

    }
}
