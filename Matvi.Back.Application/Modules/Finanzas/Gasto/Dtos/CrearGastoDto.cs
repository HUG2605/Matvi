using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Finanzas.Gasto.Dtos
{
    public class CrearGastoDto:IIdDto<int>
    {
        #region Implementacion de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion
        public byte CtSucu_Id { get; set; }
        public short CtUsua_Id { get; set; }

        [Display(Name = "Caja")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public int IdOpCorteCaja { get; set; }

        [Display(Name ="Tipo de gasto")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public byte CtTipoGast_Id { get; set; }

        [Display(Name = "Proveedor")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public string ProveedorFld { get; set; }

        [Display(Name = "Medio de pago")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public byte CtMediPago_IdFld { get; set; }

        [Display(Name = "Concepto")]
        [StringLength(1500, ErrorMessage = "El valor para {0} no debe de exceder los {1} caracteres.")]
        public string? ConceptoFld { get; set; }

        [Display(Name = "Notas")]
        [StringLength(500, ErrorMessage = "El valor para {0} no debe de exceder los {1} caracteres.")]
        public string? NotasGastoFld { get; set; }

        [Display(Name = "Importe")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor para {0} debe de ser mayor o igual a {1}.")]
        public decimal ImporteFld { get; set; }

        [Display(Name = "Cuenta bancaria")]
        public byte? CtCuentaBancaria_IdFld { get; set; }


    }
}
