using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para agregar producto pedido.
    /// </summary>
public class AgregarProductoPedidoDto : IIdDto<int>, IProductoBaseDto, IPresentacionProductoBaseDto
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

        #region implementacion de IProductoBaseDto

                /// <summary>
        /// Texto descriptivo o detalle de ct prod nombre.
        /// </summary>
public string CtProd_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa ct prod especificacion tecnica.
        /// </summary>
public string? CtProd_EspecificacionTecnica { get; set; }
                /// <summary>
        /// Propiedad que representa ct prod marca.
        /// </summary>
public string? CtProd_Marca { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de ct prod nombre completo.
        /// </summary>
public string CtProd_NombreCompleto => this.GetNombreCompleto();

        #endregion

        #region Implementacion de IPresentacionProductoBaseDto

                /// <summary>
        /// Propiedad que representa ct pres prod id ct producto.
        /// </summary>
public int CtPresProd_IdCtProducto { get; set; }

                /// <summary>
        /// Texto descriptivo o detalle de ct pres prod nombre.
        /// </summary>
public string CtPresProd_Nombre { get; set; } = string.Empty;

                /// <summary>
        /// Propiedad que representa ct pres prod factor ubi.
        /// </summary>
public decimal CtPresProd_FactorUBI { get; set; }

        #endregion

                /// <summary>
        /// Propiedad que representa ct pres prod id.
        /// </summary>
public int CtPresProd_IdFld { get; set; }

        /// <summary>
        /// Permite pedir el producto aunque no haya existencia.
        /// </summary>
        public bool CtProd_BajoPedido { get; set; }

        /// <summary>
        /// Indica si el vendedor puede entregar el producto directamente en mostrador.
        /// </summary>
        public bool CtProd_EntregaMostrador { get; set; }

        /// <summary>
        /// Indica si el producto está regularizado en la sucursal actual.
        /// </summary>
        public bool RlProdSucu_Regularizado { get; set; }

                /// <summary>
        /// Propiedad que representa ct unid base inve abreviatura.
        /// </summary>
public string CtUnidBaseInve_Abreviatura { get; set; } = string.Empty;

                /// <summary>
        /// Cantidad o volumen de unidades de cantidad.
        /// </summary>
public decimal CantidadFld { get; set; }

                /// <summary>
        /// Monto económico correspondiente a precio.
        /// </summary>
public decimal Precio { get; set; }

                /// <summary>
        /// Monto económico correspondiente a importe.
        /// </summary>
public decimal Importe { get; set; }

        /// <summary>
        /// Precio normal (sin mayoreo) capturado al momento de agregar el producto.
        /// Se conserva para referencia visual en el grid cuando se aplica mayoreo.
        /// </summary>
        public decimal PrecioNormal { get; set; }

        /// <summary>
        /// Precio de mayoreo configurado en la presentación (si existe).
        /// </summary>
        public decimal? PrecioMayoreo { get; set; }

        /// <summary>
        /// Cantidad mínima de piezas a partir de la cual aplica el precio de mayoreo.
        /// </summary>
        public short? UnidadesMayoreo { get; set; }

        /// <summary>
        /// Indica si al producto se le aplicó precio de mayoreo en función de la cantidad.
        /// </summary>
        public bool AplicoMayoreo { get; set; }

        /// <summary>
        /// Indica si el vendedor marcó el producto como entregado en mostrador.
        /// </summary>
        public bool EntregadoMostrador { get; set; }

    }
}
