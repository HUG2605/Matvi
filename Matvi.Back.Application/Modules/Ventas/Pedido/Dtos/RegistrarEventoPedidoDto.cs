using System.ComponentModel.DataAnnotations;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Dtos
{
    /// <summary>
    /// Datos para registrar un evento en el historial de una línea de pedido.
    /// Una línea puede recibir N eventos del mismo tipo (ej. varios SURTIDO
    /// parciales). Cada evento impacta los cachés de la línea (CantidadSurtida,
    /// CantidadCancelada, CantidadEntregada, CantidadRechazada) y recalcula
    /// el estatus de la misma.
    /// </summary>
    public class RegistrarEventoPedidoDto : IIdDto<int>
    {
        #region Implementacion de IIdDto

        /// <summary>
        /// Id del pedido al que pertenece la línea. Se usa para emitir el
        /// evento a nivel header (calcular si el pedido se cierra).
        /// </summary>
        [Display(Name = "Pedido")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public int Id { get; set; }

        public string SafeId => Id.GetSafeId();

        #endregion

        #region Evento

        /// <summary>
        /// Id de la línea de pedido sobre la que se registra el evento.
        /// </summary>
        [Display(Name = "Línea de pedido")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public int IdOpPedidoDetalle { get; set; }

        public int OpPediDeta_Id
        {
            get => IdOpPedidoDetalle;
            set => IdOpPedidoDetalle = value;
        }

        /// <summary>
        /// Tipo de evento (RESERVADO, SURTIDO, ENTREGADO, RECHAZADO, CANCELADO).
        /// </summary>
        [Display(Name = "Tipo de evento")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public TipoEventoPedidoDetalleEnm TipoEvento { get; set; }

        /// <summary>
        /// Cantidad afectada por este evento (no acumulada). Siempre en la unidad
        /// de la presentación con la que se vendió la línea.
        /// </summary>
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [Range(0.00001, double.MaxValue, ErrorMessage = "El valor para {0} debe ser mayor a cero.")]
        public decimal Cantidad { get; set; }

        /// <summary>
        /// Notas opcionales. Ej. motivo del rechazo, observaciones de entrega.
        /// </summary>
        [Display(Name = "Notas")]
        [StringLength(300, ErrorMessage = "El valor para {0} no debe de exceder los {1} caracteres.")]
        public string? Notas { get; set; }

        #endregion

        #region Bitacora

        /// <summary>
        /// Id del usuario que ejecuta la acción.
        /// </summary>
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public short CtUsua_Id { get; set; }

        #endregion

        #region Opcionales (cuando el evento genera movimiento de inventario)

        /// <summary>
        /// Id del movimiento de inventario relacionado, si el evento ya lo generó
        /// en una operación separada (SURTIDO y RECHAZADO típicamente).
        /// </summary>
        public int? IdOpMovimientoInventario { get; set; }

        public int? OpMoviInve_Id
        {
            get => IdOpMovimientoInventario;
            set => IdOpMovimientoInventario = value;
        }

        #endregion
    }
}