using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos
{
    public class ListarCorteCajaDto : IIdDto<int>, IFolioDto
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

        public DateTime FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }
        public string CtUsua_Nombre { get; set; } = string.Empty;
        public decimal FondoInicial { get; set; }
        public decimal? Diferencia { get; set; }
        public bool Activa { get; set; }
    }
}