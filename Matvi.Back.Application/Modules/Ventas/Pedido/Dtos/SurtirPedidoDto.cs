using System.ComponentModel.DataAnnotations;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Dtos
{
    /// <summary>
    /// Datos para surtir un pedido.
    /// </summary>
    public class SurtirPedidoDto : IIdDto<int>
    {
        #region Implementacion de IIdDto

        /// <summary>
        /// Id del pedido a surtir.
        /// </summary>
        [Display(Name = "Pedido")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public int Id { get; set; }

        public string SafeId => Id.GetSafeId();

        #endregion

        #region Surtido

        /// <summary>
        /// Modalidad de surtido: REPARTIDOR (1) o DIRECTO (2).
        /// </summary>
        [Display(Name = "Tipo de surtido")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public TipoSurtidoEnm TipoSurtido { get; set; }

        /// <summary>
        /// Id del usuario repartidor. Requerido solo cuando TipoSurtido == REPARTIDOR.
        /// </summary>
        [Display(Name = "Repartidor")]
        public short? IdCtUsuarioRepartidor { get; set; }

        public short? CtUsuaRepa_Id
        {
            get => IdCtUsuarioRepartidor;
            set => IdCtUsuarioRepartidor = value;
        }

        #endregion

        #region Lineas a surtir

        /// <summary>
        /// Líneas con las cantidades a surtir en esta operación.
        /// Si la lista está vacía, se surte el remanente total pendiente de cada línea.
        /// </summary>
        [Display(Name = "Líneas")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public List<SurtirPedidoLineaDto> Lineas { get; set; } = [];

        #endregion

        #region Bitacora

        /// <summary>
        /// Id del usuario que ejecuta la operación de surtido (almacenista).
        /// </summary>
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public short CtUsua_Id { get; set; }

        #endregion
    }

    /// <summary>
    /// Cantidad a surtir de una línea específica de pedido.
    /// </summary>
    public class SurtirPedidoLineaDto
    {
        /// <summary>
        /// Id de la línea de pedido (OpPedidoDetalle).
        /// </summary>
        [Display(Name = "Línea de pedido")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public int IdOpPedidoDetalle { get; set; }

        /// <summary>
        /// Cantidad a surtir en esta operación (en unidad de la presentación vendida).
        /// </summary>
        [Display(Name = "Cantidad a surtir")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor para {0} debe ser mayor o igual a {1}.")]
        public decimal CantidadASurtirFld { get; set; }

        public decimal CantidadASurtir
        {
            get => CantidadASurtirFld;
            set => CantidadASurtirFld = value;
        }
    }
}
