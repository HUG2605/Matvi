// DetallarCobroDetalleDto.cs
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Finanzas.Cobro.Dtos
{
    public class DetallarCobroDetalleDto : IIdDto<int>, IFolioDto
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

        public DateTime FechaPedido { get; set; }
        public decimal TotalPedido { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal MontoAplicado { get; set; }
        public decimal SaldoPendiente => TotalPedido - TotalPagado;
    }
}