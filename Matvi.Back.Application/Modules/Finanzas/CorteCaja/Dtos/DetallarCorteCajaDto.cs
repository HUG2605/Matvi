using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos
{
    public class DetallarCorteCajaDto : IIdDto<int>, IFolioDto
    {
        #region Implementacion de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        #region Implementacion de IFolioDto

        public string CtSucu_Clave { get; set; } = string.Empty;
        public int Numero { get; set; }
        public string Folio => this.GetFolio(TipoConsecutivoEnm.CORTE_DE_CAJA);

        #endregion

        public string CtSucu_Nombre { get; set; } = string.Empty;
        public string CtUsua_Nombre { get; set; } = string.Empty;
        public DateTime FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }
        public decimal FondoInicial { get; set; }
        public decimal? TotalCobros { get; set; }
        public decimal? TotalCobrosEfectivo { get; set; }
        public decimal? TotalCobrosBancario { get; set; }
        public decimal? TotalEntregasRepartidor { get; set; }
        public decimal? TotalGastos { get; set; }
        public decimal? TotalGastosEfectivo { get; set; }
        public decimal? TotalGastosBancario { get; set; }
        public decimal? TotalPagos { get; set; }
        public decimal? TotalPagosEfectivo { get; set; }
        public decimal? TotalPagosBancario { get; set; }
        public decimal? FondoFinalSistema { get; set; }
        public decimal? FondoFinalContado { get; set; }
        public decimal? Diferencia { get; set; }
        public decimal? TotalVentas { get; set; }
        public decimal? CostoVentasReal { get; set; }
        public decimal? CostoVentasRegularizado { get; set; }
        public decimal? VentasSinCostear { get; set; }
        public decimal? UtilidadBruta { get; set; }
        public decimal? UtilidadNeta { get; set; }
        public decimal? PorcentajeVentasSinCostear { get; set; }
        public decimal? PorcentajeCoberturaGastos { get; set; }
        public decimal? GastosAbsorbidosDeCapital { get; set; }
        public string? Notas { get; set; }
    }
}