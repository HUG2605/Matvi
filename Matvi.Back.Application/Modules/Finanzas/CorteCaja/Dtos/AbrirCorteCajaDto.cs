using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para abrir corte caja.
    /// </summary>
public class AbrirCorteCajaDto
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
        /// Propiedad que representa fondo inicial.
        /// </summary>
[Display(Name = "Fondo inicial")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor para {0} debe ser mayor o igual a cero.")]
        public decimal FondoInicial { get; set; }

                /// <summary>
        /// Propiedad que representa notas.
        /// </summary>
[Display(Name = "Notas")]
        [StringLength(500, ErrorMessage = "El valor para {0} no debe de exceder los {1} caracteres.")]
        public string? Notas { get; set; }
    }
}