using System;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos
{
    public class ListarGastoCajaDto : IIdDto<int>, IFolioDto
    {
        #region Implementación de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        #region Implementacion de IFolioDto

        public string CtSucu_Clave { get; set; } = string.Empty;
        public int Numero { get; set; }
        public string Folio => this.GetFolio(TipoConsecutivoEnm.GASTO);

        #endregion

        public DateTime FechaPago { get; set; }
        public string CtTipoGast_Nombre { get; set; } = string.Empty;
        public string Proveedor { get; set; } = string.Empty;
        public string Concepto { get; set; } = string.Empty;
        public decimal Importe { get; set; }
        public string CtMedioPago_Nombre { get; set; } = string.Empty;
        public string Notas { get; set; } = string.Empty;
        public bool EsBancario { get; set; }
    }
}
