using Matvi.Back.Application.Common.Constants;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Configuracion.Empresa.Dtos
{
    public class EditarEmpresaDto : IIdDto<byte>
    {
        #region Implementacion de IIdDto

        public byte Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public required string NombreFld { get; set; }

        [Display(Name = "Logotipo")]
        [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Solo se permiten archivos con extensión .jpg")]
        public string? LogotipoFld { get; set; }

        [Display(Name = "Teléfono")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        [RegularExpression(RegExp.Numerico, ErrorMessage = "El valor para {0} es incorrecto.")]
        public string? TelefonoFld { get; set; }

    }
}
