using System.ComponentModel.DataAnnotations;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Dtos
{
    /// <summary>
    /// Datos para cambiar el estatus de header de un pedido y dejar auditoría en
    /// OpPedidoEstatus. Usado por los orquestadores (EntregarPedido, CancelarPedido, etc.)
    /// cuando el último evento de línea cierra el pedido.
    /// </summary>
    public class AjustarEstatusPedidoDto : IIdDto<int>
    {
        #region Implementacion de IIdDto

        /// <summary>
        /// Id del pedido cuyo estatus se ajusta.
        /// </summary>
        [Display(Name = "Pedido")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public int Id { get; set; }

                /// <summary>
        /// Identificador único de safe id.
        /// </summary>
public string SafeId => Id.GetSafeId();

        #endregion

        #region Generales

        /// <summary>
        /// Nuevo estatus de header a aplicar.
        /// </summary>
        [Display(Name = "Estatus")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public EstatusPedidoEnm NuevoEstatus { get; set; }

        /// <summary>
        /// Nota que se registra en OpPedidoEstatus.Notas para la transición.
        /// </summary>
        [Display(Name = "Notas")]
        [StringLength(250, ErrorMessage = "El valor para {0} no debe de exceder los {1} caracteres.")]
        public string? Notas { get; set; }

        #endregion

        #region Bitacora

                /// <summary>
        /// Identificador único de ct usua id.
        /// </summary>
[Display(Name = "Usuario")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public short CtUsua_Id { get; set; }

        #endregion
    }
}