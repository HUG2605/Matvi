using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para detallar pedido.
    /// </summary>
public class DetallarPedidoDto : IIdDto<int>, IFolioDto
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

        #region Generales

                /// <summary>
        /// Texto descriptivo o detalle de ct sucu nombre.
        /// </summary>
public string CtSucu_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Fecha y hora correspondiente a fecha.
        /// </summary>
public DateTime Fecha { get; set; }

                /// <summary>
        /// Identificador único de id ct cliente.
        /// </summary>
public int? IdCtCliente { get; set; }
                /// <summary>
        /// Identificador único de ct clie id.
        /// </summary>
public int? CtClie_Id
        {
            get => IdCtCliente;
            set => IdCtCliente = value;
        }

                /// <summary>
        /// Texto descriptivo o detalle de ct clie nombre.
        /// </summary>
public string? CtClie_Nombre { get; set; }

                /// <summary>
        /// Texto descriptivo o detalle de nombre cliente mostrador.
        /// </summary>
public string? NombreClienteMostrador { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de cliente mostrador nombre.
        /// </summary>
public string? ClienteMostradorNombre
        {
            get => NombreClienteMostrador;
            set => NombreClienteMostrador = value;
        }

                /// <summary>
        /// Propiedad que representa telefono cliente mostrador.
        /// </summary>
public string? TelefonoClienteMostrador { get; set; }
                /// <summary>
        /// Propiedad que representa cliente mostrador telefono.
        /// </summary>
public string? ClienteMostradorTelefono
        {
            get => TelefonoClienteMostrador;
            set => TelefonoClienteMostrador = value;
        }

                /// <summary>
        /// Texto descriptivo o detalle de ct cana capt nombre.
        /// </summary>
public string CtCanaCapt_Nombre { get; set; } = string.Empty;

                /// <summary>
        /// Identificador único de id ct estatus pedido.
        /// </summary>
public byte IdCtEstatusPedido { get; set; }
                /// <summary>
        /// Identificador único de ct esta pedi id.
        /// </summary>
public byte CtEstaPedi_Id
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

                /// <summary>
        /// Propiedad que representa entrega domicilio.
        /// </summary>
public bool EntregaDomicilio { get; set; }
                /// <summary>
        /// Propiedad que representa direccion entrega.
        /// </summary>
public string? DireccionEntrega { get; set; }
                /// <summary>
        /// Propiedad que representa notas.
        /// </summary>
public string? Notas { get; set; }

                /// <summary>
        /// Monto económico correspondiente a total.
        /// </summary>
public decimal Total { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total pagado.
        /// </summary>
public decimal TotalPagado { get; set; }
                /// <summary>
        /// Monto económico correspondiente a saldo pendiente.
        /// </summary>
public decimal SaldoPendiente => Total - TotalPagado;

        #endregion

        #region Productos

                /// <summary>
        /// Colección de elementos asociados a productos.
        /// </summary>
public List<DetallarPedidoDetalleDto> Productos { get; set; } = [];

        #endregion

        #region Cobros

                /// <summary>
        /// Colección de elementos asociados a cobros.
        /// </summary>
public List<DetallarPedidoCobroDto> Cobros { get; set; } = [];

        #endregion

        #region Lotes

                /// <summary>
        /// Colección de elementos asociados a lotes.
        /// </summary>
public List<DetallarPedidoLoteDto> Lotes { get; set; } = [];

        #endregion

        #region Historial de Estatus

                /// <summary>
        /// Colección de elementos asociados a historial estatus.
        /// </summary>
public List<DetallarPedidoEstatusDto> HistorialEstatus { get; set; } = [];

        #endregion
    }
}