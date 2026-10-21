using System;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;
using Matvi.Back.Infrastructure.Persistence.Configurations;

namespace Matvi.Back.Application.Modules.Inventarios.Movimiento.Dtos
{
    public class DetalleMovimientoDto : IIdDto<int>, IFolioDto, IProductoBaseDto, IPresentacionProductoBaseDto
    {
        #region Implementacion de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        #region Implementacion de IFolioDto

        public string CtSucu_Clave { get; set; } = string.Empty;
        public int Numero { get; set; }
        public string Folio => this.GetFolio(TipoConsecutivoEnm.MOVIMIENTO_DE_INVENTARIO);
        public string CtSucu_Nombre { get; set; } = string.Empty;

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

        public string CtTipoMovimiento_Nombre { get; set; } = string.Empty;
        public decimal CantidadUBI { get; set; }
        public DateTime Fecha { get; set; }

        #region Referencias a otros documentos

        public int? IdOpPedidoDetalleEvento { get; set; }
        public int? IdOpPedidoDetalle { get; set; }
        public int? IdOpCompraDetalle { get; set; }
        public int? IdOpTraspasoDetalle { get; set; }
        public int? IdOpLote { get; set; }

        #endregion

        #region Costos

        public decimal? CostoUBI { get; set; }
        public decimal? CostoTotal { get; set; }

        #endregion

        #region Usuario

        public string CtUsuario_Nombre { get; set; } = string.Empty;

        #endregion

        #region Unidad Base de Inventario

        public string CtUnidBaseInve_Abreviatura { get; set; } = string.Empty;

        #endregion
    }
}