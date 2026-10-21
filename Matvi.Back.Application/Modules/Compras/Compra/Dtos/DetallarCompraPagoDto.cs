using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Compras.Compra.Dtos
{
    public class DetallarCompraPagoDto:IIdDto<int>, IFolioDto
    {
        #region Implementacion de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        #region Implementacion de IFolioDto

        public string CtSucu_Clave { get; set; } = string.Empty;
        public int Numero { get; set; }
        public string Folio => this.GetFolio(TipoConsecutivoEnm.PAGO);

        #endregion

        public DateTime Fecha { get; set; }
        public decimal MontoTotal { get; set; }
        public string CtMediPago_Nombre { get; set; }=string.Empty;
        public string CtUsuaPaga_Nombre { get; set; }=string.Empty;
        public decimal MontoAplicado { get; set; }
        public decimal SaldoInicial { get; set; }
        public decimal SaldoFinal => SaldoInicial - MontoAplicado;
    }
}
