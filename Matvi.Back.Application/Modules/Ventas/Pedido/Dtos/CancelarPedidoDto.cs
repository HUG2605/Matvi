using System.ComponentModel.DataAnnotations;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Dtos
{
    /// <summary>
    /// Datos para cancelar mercancía de un pedido administrativamente.
    /// Solo permite cancelar cantidades que nunca fueron surtidas (Cantidad - Surtida - Cancelada).
    /// </summary>
    public class CancelarPedidoDto : IIdDto<int>
    {
        #region Implementacion de IIdDto

                /// <summary>
        /// Identificador único de id.
        /// </summary>
[Required(ErrorMessage = "El valor para {0} es requerido.")]
        public int Id { get; set; }

                /// <summary>
        /// Identificador único de safe id.
        /// </summary>
public string SafeId => Id.GetSafeId();

        #endregion

        #region Generales

                /// <summary>
        /// Propiedad que representa motivo.
        /// </summary>
[Display(Name = "Motivo")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(500, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public string MotivoFld { get; set; } = string.Empty;

                /// <summary>
        /// Propiedad que representa motivo.
        /// </summary>
public string Motivo
        {
            get => MotivoFld;
            set => MotivoFld = value;
        }

        #endregion

        #region Lineas

                /// <summary>
        /// Colección de elementos asociados a lineas.
        /// </summary>
public List<CancelarPedidoLineaDto> Lineas { get; set; } = [];

        #endregion

        #region Caja

                /// <summary>
        /// Identificador único de ct sucu id.
        /// </summary>
[Display(Name = "Sucursal")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public byte CtSucu_Id { get; set; }

                /// <summary>
        /// Identificador único de id ct sucursal.
        /// </summary>
public byte IdCtSucursal
        {
            get => CtSucu_Id;
            set => CtSucu_Id = value;
        }

        /// <summary>Id de la caja abierta del usuario; requerido solo si el pedido tiene saldo pagado para registrar la devolución.</summary>
        public int? IdOpCorteCajaFld { get; set; }

                /// <summary>
        /// Identificador único de id op corte caja.
        /// </summary>
public int? IdOpCorteCaja
        {
            get => IdOpCorteCajaFld;
            set => IdOpCorteCajaFld = value;
        }

        #endregion

        #region Bitacora

                /// <summary>
        /// Identificador único de ct usua id.
        /// </summary>
public short CtUsua_Id { get; set; }

                /// <summary>
        /// Identificador único de id ct usuario.
        /// </summary>
public short IdCtUsuario
        {
            get => CtUsua_Id;
            set => CtUsua_Id = value;
        }

        #endregion
    }

    /// <summary>
    /// Cantidad a cancelar por línea de pedido.
    /// </summary>
    public class CancelarPedidoLineaDto
    {
                /// <summary>
        /// Identificador único de id op pedido detalle.
        /// </summary>
[Display(Name = "Línea de pedido")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public int IdOpPedidoDetalle { get; set; }

                /// <summary>
        /// Cantidad o volumen de unidades de cantidad a cancelar.
        /// </summary>
[Display(Name = "Cantidad a cancelar")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor para {0} debe ser mayor o igual a {1}.")]
        public decimal CantidadACancelarFld { get; set; }

                /// <summary>
        /// Cantidad o volumen de unidades de cantidad a cancelar.
        /// </summary>
public decimal CantidadACancelar
        {
            get => CantidadACancelarFld;
            set => CantidadACancelarFld = value;
        }
    }
}
