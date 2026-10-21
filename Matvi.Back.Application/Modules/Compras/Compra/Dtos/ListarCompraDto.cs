using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Compras.Compra.Dtos
{
    public class ListarCompraDto: IIdDto<int>, IFolioDto
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

        public DateTime Fecha { get; set; }
        public string CtProv_Nombre { get; set; } = string.Empty;
        public bool Pagado { get; set; }
        public decimal Total { get; set; }
        public decimal TotalPagado { get; set; }
    }
}
