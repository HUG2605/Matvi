using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para detallar pedido detalle.
    /// </summary>
public class DetallarPedidoDetalleDto : IIdDto<int>, IFolioDto, IProductoBaseDto, IPresentacionProductoBaseDto
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

        #region Implementacion de IFolioDto

                /// <summary>
        /// Propiedad que representa ct sucu clave.
        /// </summary>
public string CtSucu_Clave { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa numero.
        /// </summary>
public int Numero { get; set; }
                /// <summary>
        /// Propiedad que representa folio.
        /// </summary>
public string Folio => this.GetFolio(TipoConsecutivoEnm.PEDIDO);

        #endregion

        #region Implementacion de IProductoBaseDto

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

        #region Cantidades

                /// <summary>
        /// Cantidad o volumen de unidades de cantidad.
        /// </summary>
public decimal Cantidad { get; set; }
                /// <summary>
        /// Cantidad o volumen de unidades de cantidad surtida.
        /// </summary>
public decimal CantidadSurtida { get; set; }
                /// <summary>
        /// Cantidad o volumen de unidades de cantidad entregada.
        /// </summary>
public decimal CantidadEntregada { get; set; }
                /// <summary>
        /// Cantidad o volumen de unidades de cantidad rechazada.
        /// </summary>
public decimal CantidadRechazada { get; set; }
                /// <summary>
        /// Cantidad o volumen de unidades de cantidad cancelada.
        /// </summary>
public decimal CantidadCancelada { get; set; }

                /// <summary>
        /// Cantidad o volumen de unidades de cantidad pendiente surtir.
        /// </summary>
public decimal CantidadPendienteSurtir => Math.Max(0m, Cantidad - CantidadSurtida - CantidadCancelada);
                /// <summary>
        /// Cantidad o volumen de unidades de cantidad por resolver.
        /// </summary>
public decimal CantidadPorResolver => Math.Max(0m, Cantidad - CantidadEntregada - CantidadRechazada - CantidadCancelada);
                /// <summary>
        /// Propiedad que representa custodia.
        /// </summary>
public decimal Custodia => Math.Max(0m, CantidadSurtida - CantidadEntregada - CantidadRechazada);

        #endregion

        #region Estatus e Importes

                /// <summary>
        /// Identificador único de id ct estatus pedido detalle.
        /// </summary>
public byte IdCtEstatusPedidoDetalle { get; set; }
                /// <summary>
        /// Identificador único de ct esta pedi deta id.
        /// </summary>
public byte CtEstaPediDeta_Id
        {
            get => IdCtEstatusPedidoDetalle;
            set => IdCtEstatusPedidoDetalle = value;
        }

                /// <summary>
        /// Texto descriptivo o detalle de ct estatus nombre.
        /// </summary>
public string CtEstatus_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Texto descriptivo o detalle de ct esta pedi deta nombre.
        /// </summary>
public string CtEstaPediDeta_Nombre
        {
            get => CtEstatus_Nombre;
            set => CtEstatus_Nombre = value;
        }

                /// <summary>
        /// Monto económico correspondiente a precio.
        /// </summary>
public decimal Precio { get; set; }
                /// <summary>
        /// Monto económico correspondiente a importe.
        /// </summary>
public decimal Importe { get; set; }
                /// <summary>
        /// Propiedad que representa ct unid base inve abreviatura.
        /// </summary>
public string CtUnidBaseInve_Abreviatura { get; set; } = string.Empty;

        #endregion
    }
}
