using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Finanzas.Cobro.Dtos
{
    public class CrearCobroDto
    {
        public byte CtSucu_Id { get; set; }
        public short CtUsua_Id { get; set; }

        [Display(Name = "Caja")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public int IdOpCorteCaja { get; set; }

        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public int CtClie_IdFld { get; set; }

        [Display(Name = "Monto")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El valor para {0} debe ser mayor a cero.")]
        public decimal MontoFld { get; set; }

        [Display(Name = "Medio de pago")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public byte CtMediPago_IdFld { get; set; }

        [Display(Name = "Notas")]
        [StringLength(500, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public string? NotasFld { get; set; }

        [Display(Name = "Cuenta bancaria")]
        public byte? CtCuentaBancaria_IdFld { get; set; }
    }
}