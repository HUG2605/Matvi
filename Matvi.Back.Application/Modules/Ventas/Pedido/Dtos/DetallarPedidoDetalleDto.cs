using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Dtos
{
    public class DetallarPedidoDetalleDto : IIdDto<int>, IFolioDto, IProductoBaseDto, IPresentacionProductoBaseDto
    {
        #region Implementacion de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        #region Implementacion de IFolioDto

        public string CtSucu_Clave { get; set; } = string.Empty;
        public int Numero { get; set; }
        public string Folio => this.GetFolio(TipoConsecutivoEnm.PEDIDO);

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

        #region Cantidades

        public decimal Cantidad { get; set; }
        public decimal CantidadSurtida { get; set; }
        public decimal CantidadEntregada { get; set; }
        public decimal CantidadRechazada { get; set; }
        public decimal CantidadCancelada { get; set; }

        public decimal CantidadPendienteSurtir => Math.Max(0m, Cantidad - CantidadSurtida - CantidadCancelada);
        public decimal CantidadPorResolver => Math.Max(0m, Cantidad - CantidadEntregada - CantidadRechazada - CantidadCancelada);
        public decimal Custodia => Math.Max(0m, CantidadSurtida - CantidadEntregada - CantidadRechazada);

        #endregion

        #region Estatus e Importes

        public byte IdCtEstatusPedidoDetalle { get; set; }
        public byte CtEstaPediDeta_Id
        {
            get => IdCtEstatusPedidoDetalle;
            set => IdCtEstatusPedidoDetalle = value;
        }

        public string CtEstatus_Nombre { get; set; } = string.Empty;
        public string CtEstaPediDeta_Nombre
        {
            get => CtEstatus_Nombre;
            set => CtEstatus_Nombre = value;
        }

        public decimal Precio { get; set; }
        public decimal Importe { get; set; }
        public string CtUnidBaseInve_Abreviatura { get; set; } = string.Empty;

        #endregion
    }
}
