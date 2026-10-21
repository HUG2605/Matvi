using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos
{
    public class CerrarCorteCajaDto : IIdDto<int>
    {
        #region Implementacion de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        public byte CtSucu_Id { get; set; }
        public short CtUsua_Id { get; set; }

        [Display(Name = "Fondo final contado")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor para {0} debe ser mayor o igual a cero.")]
        public decimal FondoFinalContado { get; set; }

        [Display(Name = "Notas de cierre")]
        [StringLength(500, ErrorMessage = "El valor para {0} no debe de exceder los {1} caracteres.")]
        public string? Notas { get; set; }
    }
}