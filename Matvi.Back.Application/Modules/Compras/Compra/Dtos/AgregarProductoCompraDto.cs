using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Compras.Compra.Dtos
{
    public class AgregarProductoCompraDto : IIdDto<int>, IProductoBaseDto, IPresentacionProductoBaseDto
    {
        #region Implementacion de IIdDto
        public int Id { get ; set ; }
        public string SafeId => Id.GetSafeId();

        #endregion

        #region Implementacion de IProductoBaseDto
        public string CtProd_Nombre { get; set; } = string.Empty;
        public string? CtProd_EspecificacionTecnica { get; set; }
        public string? CtProd_Marca { get; set; }
        public string CtProd_NombreCompleto => this.GetNombreCompleto();

        #endregion

        #region Implementacion de IPresentacionProductoBaseDto

        public int CtPresProd_IdCtProducto { get; set; }

        public string CtPresProd_Nombre { get; set; } = string.Empty;

        public decimal CtPresProd_FactorUBI { get; set; }

        #endregion

        public int CtPresProd_IdFld { get; set; }
        public string CtUnidBaseInve_Abreviatura { get; set; } = string.Empty;
        public decimal CantidadFld { get; set; }
        public decimal CostoUP { get; set; }
        public decimal CostoUBI { get; set; }
        public decimal Importe { get; set; }

    }
}
