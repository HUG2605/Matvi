using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Finanzas.Pago.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para crear pago.
    /// </summary>
public class CrearPagoDto
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
        /// Propiedad que representa ct prov id.
        /// </summary>
[Display(Name = "Proveedor")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public int CtProv_IdFld { get; set; }

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
        /// Propiedad que representa notas pago.
        /// </summary>
[Display(Name = "Notas")]
        [StringLength(500, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public string? NotasPagoFld { get; set; }

                /// <summary>
        /// Propiedad que representa ct cuenta bancaria id.
        /// </summary>
[Display(Name = "Cuenta bancaria")]
        public byte? CtCuentaBancaria_IdFld { get; set; }

    }
}
