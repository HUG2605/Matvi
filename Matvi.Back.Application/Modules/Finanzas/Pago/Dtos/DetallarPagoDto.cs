using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Finanzas.Pago.Dtos
{
    public class DetallarPagoDto : IIdDto<int>, IFolioDto
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

        public string CtSucu_Nombre { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string CtProv_Nombre { get; set; } = string.Empty;
        public decimal MontoTotal { get; set; }
        public string CtMedioPago_Nombre { get; set; } = string.Empty;
        public string CtUsuarioPaga_Nombre { get; set; } = string.Empty;
        public string? Notas { get; set; }

        public List<DetallarPagoDetalleDto> Compras { get; set; } = [];
    }
}