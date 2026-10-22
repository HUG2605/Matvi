using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Finanzas.Gasto.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para crear gasto.
    /// </summary>
public class CrearGastoDto:IIdDto<int>
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
        /// Identificador único de ct tipo gast id.
        /// </summary>
[Display(Name ="Tipo de gasto")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public byte CtTipoGast_Id { get; set; }

                /// <summary>
        /// Propiedad que representa proveedor.
        /// </summary>
[Display(Name = "Proveedor")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public string ProveedorFld { get; set; }

                /// <summary>
        /// Propiedad que representa ct medi pago id.
        /// </summary>
[Display(Name = "Medio de pago")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public byte CtMediPago_IdFld { get; set; }

                /// <summary>
        /// Propiedad que representa concepto.
        /// </summary>
[Display(Name = "Concepto")]
        [StringLength(1500, ErrorMessage = "El valor para {0} no debe de exceder los {1} caracteres.")]
        public string? ConceptoFld { get; set; }

                /// <summary>
        /// Propiedad que representa notas gasto.
        /// </summary>
[Display(Name = "Notas")]
        [StringLength(500, ErrorMessage = "El valor para {0} no debe de exceder los {1} caracteres.")]
        public string? NotasGastoFld { get; set; }

                /// <summary>
        /// Monto económico correspondiente a importe.
        /// </summary>
[Display(Name = "Importe")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor para {0} debe de ser mayor o igual a {1}.")]
        public decimal ImporteFld { get; set; }

                /// <summary>
        /// Propiedad que representa ct cuenta bancaria id.
        /// </summary>
[Display(Name = "Cuenta bancaria")]
        public byte? CtCuentaBancaria_IdFld { get; set; }


    }
}
