using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Seguridad.ChangePassword.Dtos
{
    public class CambiarContraseniaDto : IIdDto<short>
    {
        #region Implementacion de IIdDto

        public short Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        [Display(Name = "Contraseña nueva")]
        [Required(ErrorMessage = "Se requiere valor para {0}.")]
        [DataType(DataType.Password)]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El valor para {0} debe ser de {1} caracteres.")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "La contraseña no cumple con los requisitos mínimos de seguridad.")]
        public string ContraseniaFld { get; set; } = string.Empty;

        [Display(Name = "Confirmar contraseña")]
        [Required(ErrorMessage = "Se requiere valor para {0}.")]
        [DataType(DataType.Password)]
        [Compare(nameof(ContraseniaFld), ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarContraseniaFld { get; set; } = string.Empty;
    }
}
