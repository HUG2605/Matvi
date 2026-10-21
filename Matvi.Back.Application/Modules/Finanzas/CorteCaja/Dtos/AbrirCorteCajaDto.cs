using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos
{
    public class AbrirCorteCajaDto
    {
        public byte CtSucu_Id { get; set; }
        public short CtUsua_Id { get; set; }

        [Display(Name = "Fondo inicial")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor para {0} debe ser mayor o igual a cero.")]
        public decimal FondoInicial { get; set; }

        [Display(Name = "Notas")]
        [StringLength(500, ErrorMessage = "El valor para {0} no debe de exceder los {1} caracteres.")]
        public string? Notas { get; set; }
    }
}