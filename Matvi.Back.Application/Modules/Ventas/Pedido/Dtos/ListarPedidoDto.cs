using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Dtos
{
    public class ListarPedidoDto : IIdDto<int>, IFolioDto
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

        public string CtClie_Nombre { get; set; } = string.Empty;

        public string CtClie_Telefono { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }

        public string CtCanaCapt_Nombre { get; set; } = string.Empty;

        public string CtEstaPedi_Nombre { get; set; } = string.Empty;

        public bool EntregaDomicilio { get; set; }

        public decimal Total { get; set; }  
    }
}
