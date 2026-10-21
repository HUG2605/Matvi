using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Dtos
{
    public class DetallarPedidoLoteDto : IIdDto<int>, IFolioDto, IProductoBaseDto, IPresentacionProductoBaseDto
    {
        #region Implementacion de IIdDto
        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();
        #endregion

        #region Implementacion de IFolioDto
        public string CtSucu_Clave { get; set; } = string.Empty;
        public int Numero { get; set; }
        public string Folio => this.GetFolio(TipoConsecutivoEnm.LOTE);
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

        public decimal CantidadInicialUBI { get; set; }
        public decimal CantidadDisponibleUBI { get; set; }
        public decimal CostoUP { get; set; }
        public decimal CostoUBI { get; set; }
        public DateTime Fecha { get; set; }
        public string CtUnidBaseInve_Abreviatura { get; set; } = string.Empty;
    }
}