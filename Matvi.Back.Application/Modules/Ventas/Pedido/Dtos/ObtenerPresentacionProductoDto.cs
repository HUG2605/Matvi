using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para obtener presentacion producto.
    /// </summary>
public class ObtenerPresentacionProductoDto : IIdDto<int>
    {
        #region Implementacion de IIdDto

                /// <summary>
        /// Identificador único de id.
        /// </summary>
public int Id { get; set; }
                /// <summary>
        /// Identificador único de safe id.
        /// </summary>
public string SafeId => Id.GetSafeId();

        #endregion

                /// <summary>
        /// Propiedad que representa factor ubi.
        /// </summary>
public decimal FactorUBI { get; set; }
                /// <summary>
        /// Propiedad que representa existencias presentacion.
        /// </summary>
public decimal ExistenciasPresentacion { get; set; }
                /// <summary>
        /// Identificador único de ct prod id.
        /// </summary>
public int CtProd_Id { get; set; }
                /// <summary>
        /// Propiedad que representa ct prod bajo pedido.
        /// </summary>
public bool CtProd_BajoPedido { get; set; }
                /// <summary>
        /// Propiedad que representa ct prod entrega mostrador.
        /// </summary>
public bool CtProd_EntregaMostrador { get; set; }
                /// <summary>
        /// Propiedad que representa rl prod sucu regularizado.
        /// </summary>
public bool RlProdSucu_Regularizado { get; set; }

        /// <summary>
        /// Indica si el vendedor puede marcar "Entregado" para este producto.
        /// Se habilita si EntregaMostrador = true (con o sin regularizar).
        /// </summary>
        public bool PuedeVendedorEntregar => CtProd_EntregaMostrador;

    }
}
