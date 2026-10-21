using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Dtos
{
    public class ObtenerPresentacionProductoDto : IIdDto<int>
    {
        #region Implementacion de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        public decimal FactorUBI { get; set; }
        public decimal ExistenciasPresentacion { get; set; }
        public int CtProd_Id { get; set; }
        public bool CtProd_BajoPedido { get; set; }
        public bool CtProd_EntregaMostrador { get; set; }
        public bool RlProdSucu_Regularizado { get; set; }

        /// <summary>
        /// Indica si el vendedor puede marcar "Entregado" para este producto.
        /// Se habilita si EntregaMostrador = true (con o sin regularizar).
        /// </summary>
        public bool PuedeVendedorEntregar => CtProd_EntregaMostrador;

    }
}
