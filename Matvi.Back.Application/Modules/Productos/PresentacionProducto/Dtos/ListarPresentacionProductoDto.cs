using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para listar producto sucursal.
    /// </summary>
public class ListarProductoSucursalDto : IIdDto<int>
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
        /// Texto descriptivo o detalle de ct pres prod descripcion.
        /// </summary>
public string CtPresProd_Descripcion { get; set; } = string.Empty;

        //EsParaCompra    bit	1	no
                /// <summary>
        /// Indica si es para compra se encuentra activo o habilitado.
        /// </summary>
public bool EsParaCompra { get; set; }

        //EsParaVenta bit	1	no
                /// <summary>
        /// Indica si es para venta se encuentra activo o habilitado.
        /// </summary>
public bool EsParaVenta { get; set; }

        //PrecioVenta decimal	9	no
                /// <summary>
        /// Monto económico correspondiente a precio venta.
        /// </summary>
public decimal PrecioVenta { get; set; }

        //PrecioMayoreo   decimal	9	yes
                /// <summary>
        /// Monto económico correspondiente a precio mayoreo.
        /// </summary>
public decimal? PrecioMayoreo { get; set; }

        //UnidadesMayoreo smallint	1	yes
                /// <summary>
        /// Propiedad que representa unidades mayoreo.
        /// </summary>
public short? UnidadesMayoreo { get; set; }

                /// <summary>
        /// Indica si es presentacion base se encuentra activo o habilitado.
        /// </summary>
public bool EsPresentacionBase { get; set; }

    }
}
