using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Finanzas.Cobro.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para crear cobro.
    /// </summary>
public class CrearCobroDto
    {
                /// <summary>
        /// Identificador único de ct sucu id.
        /// </summary>
public byte CtSucu_Id { get; set; }
                /// <summary>
        /// Identificador único de ct usua id.
        /// </summary>
public short CtUsua_Id { get; set; }

                /// <summary>
        /// Identificador único de id op corte caja.
        /// </summary>
[Display(Name = "Caja")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public int IdOpCorteCaja { get; set; }

                /// <summary>
        /// Propiedad que representa ct clie id.
        /// </summary>
[Display(Name = "Cliente")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public int CtClie_IdFld { get; set; }

                /// <summary>
        /// Monto económico correspondiente a monto.
        /// </summary>
[Display(Name = "Monto")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El valor para {0} debe ser mayor a cero.")]
        public decimal MontoFld { get; set; }

                /// <summary>
        /// Propiedad que representa ct medi pago id.
        /// </summary>
[Display(Name = "Medio de pago")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public byte CtMediPago_IdFld { get; set; }

                /// <summary>
        /// Propiedad que representa notas.
        /// </summary>
[Display(Name = "Notas")]
        [StringLength(500, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public string? NotasFld { get; set; }

                /// <summary>
        /// Propiedad que representa ct cuenta bancaria id.
        /// </summary>
[Display(Name = "Cuenta bancaria")]
        public byte? CtCuentaBancaria_IdFld { get; set; }
    }
}