using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para crear pedido.
    /// </summary>
public class CrearPedidoDto : IIdDto<int>
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

        #region Generales

                /// <summary>
        /// Identificador único de ct sucu id.
        /// </summary>
public byte CtSucu_Id { get; set; }

                /// <summary>
        /// Propiedad que representa ct clie id.
        /// </summary>
[Display(Name = "Cliente")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public required int CtClie_IdFld { get; set; }

                /// <summary>
        /// Texto descriptivo o detalle de cliente mostrador nombre.
        /// </summary>
[Display(Name = "Nombre de cliente mostrador")]
        [StringLength(50, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public string? ClienteMostradorNombreFld { get; set; }

                /// <summary>
        /// Propiedad que representa cliente mostrador telefono.
        /// </summary>
[Display(Name = "Teléfono de cliente mostrador")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "El valor para {0} debe ser de {1} caracteres.")]
        public string? ClienteMostradorTelefonoFld { get; set; }

                /// <summary>
        /// Propiedad que representa ct cana capt id.
        /// </summary>
[Display(Name = "Canal de captura")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public required byte CtCanaCapt_IdFld { get; set; }

        #endregion

        #region Producto

                /// <summary>
        /// Colección de elementos asociados a productos.
        /// </summary>
public List<AgregarProductoPedidoDto>? Productos { get; set; }

        #endregion

        #region Entrega

                /// <summary>
        /// Propiedad que representa entrega domicilio.
        /// </summary>
[Display(Name = "Entregar a domicilio")]
        public bool EntregaDomicilioFld { get; set; }

                /// <summary>
        /// Propiedad que representa direccion entrega.
        /// </summary>
[Display(Name = "Dirección de entrega")]
        [StringLength(250, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public string? DireccionEntregaFld { get; set; }

                /// <summary>
        /// Propiedad que representa notas.
        /// </summary>
[Display(Name = "Notas")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(250, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public required string NotasFld { get; set; }

        #endregion

        #region Pago

                /// <summary>
        /// Propiedad que representa ct medi pago id.
        /// </summary>
[Display(Name = "Medio de pago")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public required byte CtMediPago_IdFld { get; set; }

                /// <summary>
        /// Propiedad que representa pago.
        /// </summary>
[Display(Name = "Pago")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor para {0} debe ser mayor o igual a {1}.")]
        public required decimal PagoFld { get; set; }

                /// <summary>
        /// Propiedad que representa ct cuenta bancaria id.
        /// </summary>
[Display(Name = "Cuenta bancaria")]
        public byte? CtCuentaBancaria_IdFld { get; set; }

        #endregion

        #region Modalidad de pago

        /// <summary>
        /// Si es true, el pedido se crea en estatus APARTADO (pagado al 100%,
        /// exclusivo de clientes registrados, no toca inventario hasta el retiro).
        /// Validado por ValidarCapturaPedidoQryHdl.
        /// </summary>
        [Display(Name = "Apartado")]
        public bool EsApartadoFld { get; set; }

        /// <summary>
        /// Fecha estimada de entrega. Opcional; útil para filtrar pedidos
        /// apartados. Persistido en OpPedido.FechaEntregaEstimada.
        /// </summary>
        [Display(Name = "Fecha de entrega estimada")]
        public DateTime? FechaEntregaEstimadaFld { get; set; }

        #endregion

        #region Bitacora

                /// <summary>
        /// Identificador único de ct usua id.
        /// </summary>
public short CtUsua_Id { get; set; }

        #endregion

    }
}
