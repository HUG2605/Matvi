using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Seguridad.ChangePassword.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para cambiar contrasenia.
    /// </summary>
public class CambiarContraseniaDto : IIdDto<short>
    {
        #region Implementacion de IIdDto

                /// <summary>
        /// Identificador único de id.
        /// </summary>
public short Id { get; set; }
                /// <summary>
        /// Identificador único de safe id.
        /// </summary>
public string SafeId => Id.GetSafeId();

        #endregion

                /// <summary>
        /// Propiedad que representa contrasenia.
        /// </summary>
[Display(Name = "Contraseña nueva")]
        [Required(ErrorMessage = "Se requiere valor para {0}.")]
        [DataType(DataType.Password)]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El valor para {0} debe ser de {1} caracteres.")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "La contraseña no cumple con los requisitos mínimos de seguridad.")]
        public string ContraseniaFld { get; set; } = string.Empty;

                /// <summary>
        /// Propiedad que representa confirmar contrasenia.
        /// </summary>
[Display(Name = "Confirmar contraseña")]
        [Required(ErrorMessage = "Se requiere valor para {0}.")]
        [DataType(DataType.Password)]
        [Compare(nameof(ContraseniaFld), ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarContraseniaFld { get; set; } = string.Empty;
    }
}
