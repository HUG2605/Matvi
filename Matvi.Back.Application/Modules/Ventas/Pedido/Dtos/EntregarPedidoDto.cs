using System.ComponentModel.DataAnnotations;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Dtos
{
    /// <summary>
    /// Datos para registrar la entrega a domicilio de un pedido por el repartidor.
    /// Por cada línea se indica cuánto se entregó al cliente y cuánto se rechazó.
    /// </summary>
    public class EntregarPedidoDto : IIdDto<int>
    {
        #region Implementacion de IIdDto

        /// <summary>
        /// Id del pedido que se entrega.
        /// </summary>
        [Display(Name = "Pedido")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public int Id { get; set; }

        public string SafeId => Id.GetSafeId();

        #endregion

        #region Generales

        /// <summary>
        /// Id del repartidor que realiza la entrega (debe coincidir con el asignado).
        /// </summary>
        [Display(Name = "Repartidor")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public short IdCtUsuarioRepartidor { get; set; }

        public short CtUsuaRepa_Id
        {
            get => IdCtUsuarioRepartidor;
            set => IdCtUsuarioRepartidor = value;
        }

        #endregion

        #region Lineas a entregar

        /// <summary>
        /// Líneas con las cantidades entregadas y rechazadas.
        /// </summary>
        [Display(Name = "Líneas")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public List<EntregarPedidoLineaDto> Lineas { get; set; } = [];

        #endregion

        #region Cobro al Entregar (Opcional)

        /// <summary>
        /// Monto cobrado al cliente durante la entrega.
        /// </summary>
        [Display(Name = "Monto de cobro")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor para {0} debe ser mayor o igual a {1}.")]
        public decimal? MontoCobroFld { get; set; }

        public decimal? MontoCobro
        {
            get => MontoCobroFld;
            set => MontoCobroFld = value;
        }

        /// <summary>
        /// Id del medio de pago utilizado (Efectivo, Transferencia, etc.).
        /// </summary>
        [Display(Name = "Medio de pago")]
        public byte? CtMediPago_IdFld { get; set; }

        public byte? IdCtMedioPago
        {
            get => CtMediPago_IdFld;
            set => CtMediPago_IdFld = value;
        }

        /// <summary>
        /// Id de la cuenta bancaria si aplica.
        /// </summary>
        [Display(Name = "Cuenta bancaria")]
        public byte? CtCuentaBancaria_IdFld { get; set; }

        public byte? IdCtCuentaBancaria
        {
            get => CtCuentaBancaria_IdFld;
            set => CtCuentaBancaria_IdFld = value;
        }

        /// <summary>
        /// Notas adicionales para el cobro.
        /// </summary>
        [Display(Name = "Notas de cobro")]
        [StringLength(500, ErrorMessage = "El valor para {0} no debe de exceder los {1} caracteres.")]
        public string? NotasCobroFld { get; set; }

        public string? NotasCobro
        {
            get => NotasCobroFld;
            set => NotasCobroFld = value;
        }

        #endregion
    }

    /// <summary>
    /// Datos de una línea al registrar la entrega.
    /// </summary>
    public class EntregarPedidoLineaDto
    {
        /// <summary>
        /// Id de la línea de pedido (OpPedidoDetalle).
        /// </summary>
        [Display(Name = "Línea de pedido")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public int IdOpPedidoDetalle { get; set; }

        /// <summary>
        /// Cantidad entregada al cliente en esta operación (presentación vendida).
        /// </summary>
        [Display(Name = "Cantidad a entregar")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor para {0} debe ser mayor o igual a {1}.")]
        public decimal CantidadEntregarFld { get; set; }

        public decimal CantidadEntregar
        {
            get => CantidadEntregarFld;
            set => CantidadEntregarFld = value;
        }

        /// <summary>
        /// Cantidad rechazada por el cliente en esta operación (presentación vendida).
        /// </summary>
        [Display(Name = "Cantidad a rechazar")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor para {0} debe ser mayor o igual a {1}.")]
        public decimal CantidadRechazarFld { get; set; }

        public decimal CantidadRechazar
        {
            get => CantidadRechazarFld;
            set => CantidadRechazarFld = value;
        }

        /// <summary>
        /// Motivo del rechazo (obligatorio si CantidadRechazar > 0).
        /// </summary>
        [Display(Name = "Motivo de rechazo")]
        [StringLength(300, ErrorMessage = "El valor para {0} no debe de exceder los {1} caracteres.")]
        public string? NotasRechazoFld { get; set; }

        public string? NotasRechazo
        {
            get => NotasRechazoFld;
            set => NotasRechazoFld = value;
        }
    }
}