using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Compras.Compra.Dtos
{
    public class DetallarCompraDto : IIdDto<int>, IFolioDto
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

        #region Generales

        public string CtSucu_Nombre { get; set; }= string.Empty;
        public DateTime Fecha { get; set; }
        public string CtProv_Nombre { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal SaldoPendiente => Total - TotalPagado;
        public string EstatusPago => TotalPagado >= Total ? "PAGADA" : "PENDIENTE";

        #endregion

        #region Productos

        public List<DetallarCompraDetalleDto> Productos { get; set; } = [];

        #endregion

        #region Lotes

        public List<DetallarCompraLoteDto> Lotes { get; set; } = [];

        #endregion

        #region Pagos

        public List<DetallarCompraPagoDto> Pagos { get; set; } = [];
        
        #endregion

    }
}
