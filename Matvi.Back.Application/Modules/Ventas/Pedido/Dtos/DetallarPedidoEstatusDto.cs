using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para detallar pedido estatus item.
    /// </summary>
public class DetallarPedidoEstatusItemDto
    {
                /// <summary>
        /// Texto descriptivo o detalle de producto nombre.
        /// </summary>
public string ProductoNombre { get; set; } = string.Empty;

                /// <summary>
        /// Texto descriptivo o detalle de ct prod nombre.
        /// </summary>
public string CtProd_Nombre
        {
            get => ProductoNombre;
            set => ProductoNombre = value;
        }

                /// <summary>
        /// Texto descriptivo o detalle de presentacion nombre.
        /// </summary>
public string PresentacionNombre { get; set; } = string.Empty;

                /// <summary>
        /// Texto descriptivo o detalle de ct pres prod nombre.
        /// </summary>
public string CtPresProd_Nombre
        {
            get => PresentacionNombre;
            set => PresentacionNombre = value;
        }

                /// <summary>
        /// Cantidad o volumen de unidades de cantidad.
        /// </summary>
public decimal Cantidad { get; set; }
    }

        /// <summary>
    /// Objeto de transferencia de datos (DTO) para detallar pedido estatus.
    /// </summary>
public class DetallarPedidoEstatusDto : IIdDto<int>
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

        #region Estatus General de Pedido

                /// <summary>
        /// Identificador único de id ct estatus pedido.
        /// </summary>
public byte? IdCtEstatusPedido { get; set; }

                /// <summary>
        /// Identificador único de ct esta pedi id.
        /// </summary>
public byte? CtEstaPedi_Id
        {
            get => IdCtEstatusPedido;
            set => IdCtEstatusPedido = value;
        }

                /// <summary>
        /// Texto descriptivo o detalle de ct estatus nombre.
        /// </summary>
public string CtEstatus_Nombre { get; set; } = string.Empty;

                /// <summary>
        /// Texto descriptivo o detalle de ct esta pedi nombre.
        /// </summary>
public string CtEstaPedi_Nombre
        {
            get => CtEstatus_Nombre;
            set => CtEstatus_Nombre = value;
        }

        #endregion

        #region Evento de Detalle de Linea

                /// <summary>
        /// Indica si es evento detalle se encuentra activo o habilitado.
        /// </summary>
public bool EsEventoDetalle { get; set; }
                /// <summary>
        /// Identificador único de id ct tipo evento pedido detalle.
        /// </summary>
public byte? IdCtTipoEventoPedidoDetalle { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de ct tipo evento nombre.
        /// </summary>
public string? CtTipoEvento_Nombre { get; set; }
                /// <summary>
        /// Colección de elementos asociados a productos.
        /// </summary>
public List<DetallarPedidoEstatusItemDto> Productos { get; set; } = [];

                /// <summary>
        /// Cantidad o volumen de unidades de cantidad lineas.
        /// </summary>
public int CantidadLineas { get; set; }

        #endregion

        #region Auditoria y Notas

                /// <summary>
        /// Fecha y hora correspondiente a fecha.
        /// </summary>
public DateTime Fecha { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de ct usuario nombre.
        /// </summary>
public string CtUsuario_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa notas.
        /// </summary>
public string? Notas { get; set; }

        #endregion

        #region Sub-Eventos Operativos

                /// <summary>
        /// Colección de elementos asociados a sub eventos.
        /// </summary>
public List<DetallarPedidoEstatusDto> SubEventos { get; set; } = [];

        #endregion
    }
}
