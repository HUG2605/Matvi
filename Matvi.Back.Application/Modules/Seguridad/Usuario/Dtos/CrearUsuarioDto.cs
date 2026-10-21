using Matvi.Back.Application.Common.Constants;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Dtos
{
    public class CrearUsuarioDto : IIdDto<short>
    {
        #region Implementacion de IIdDto

        public short Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        //IdCatRol    tinyint	1	no
        [Display(Name = "Rol")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public required byte RolIdFld { get; set; }

        //Nombre  varchar	25	no
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(25, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        [RegularExpression(RegExp.Alfabetico, ErrorMessage = "El valor para {0} es incorrecto.")]
        public required string NombreFld { get; set; }

        //PrimerApellido  varchar	25	no
        [Display(Name = "Primer apellido")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(25, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        [RegularExpression(RegExp.Alfabetico, ErrorMessage = "El valor para {0} es incorrecto.")]
        public required string PrimerApellidoFld { get; set; }

        //SegundoApellido varchar	25	no
        [Display(Name = "Segundo apellido")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(25, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        [RegularExpression(RegExp.Alfabetico, ErrorMessage = "El valor para {0} es incorrecto.")]
        public required string SegundoApellidoFld { get; set; }

        //Direccion   varchar	100	no
        [Display(Name = "Dirección")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(100, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public required string DireccionFld { get; set; }

        //Telefono   varchar	10	no
        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "El valor para {0} debe ser de {1} caracteres.")]
        [RegularExpression(RegExp.Numerico, ErrorMessage = "El valor para {0} es incorrecto.")]
        public required string TelefonoFld { get; set; }

        public List<RelacionarSucursalDto>? Sucursales { get; set; }
    }
}
