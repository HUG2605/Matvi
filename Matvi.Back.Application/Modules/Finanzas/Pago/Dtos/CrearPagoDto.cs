using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Finanzas.Pago.Dtos
{
    public class CrearPagoDto
    {

        public byte CtSucu_Id { get; set; }
        public short CtUsua_Id { get; set; }

        [Display(Name = "Caja")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public int IdOpCorteCaja { get; set; }

        [Display(Name = "Proveedor")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public int CtProv_IdFld { get; set; }

        [Display(Name = "Monto")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El valor para {0} debe ser mayor a cero.")]
        public decimal MontoFld { get; set; }

        [Display(Name = "Medio de pago")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public byte CtMediPago_IdFld { get; set; }

        [Display(Name = "Notas")]
        [StringLength(500, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public string? NotasPagoFld { get; set; }

        [Display(Name = "Cuenta bancaria")]
        public byte? CtCuentaBancaria_IdFld { get; set; }

    }
}
