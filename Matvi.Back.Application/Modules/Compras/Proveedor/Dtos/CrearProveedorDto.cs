using Matvi.Back.Application.Common.Constants;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Compras.Proveedor.Dtos
{
    public class CrearProveedorDto : IIdDto<int>
    {
        #region Implementacion de IIdDto

        public int Id { get; set; }
        public string SafeId => Id.GetSafeId();

        #endregion

        //Nombre varchar	50	no
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public required string NombreFld { get; set; }

        //Direccion varchar	50	no
        [Display(Name = "Dirección")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(100, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public required string DireccionFld { get; set; }

        //Telefono	varchar	10	no
        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "El valor para {0} debe ser de {1} caracteres.")]
        [RegularExpression(RegExp.Numerico, ErrorMessage = "El valor para {0} es incorrecto.")]
        public required string TelefonoFld { get; set; }

        //NombreContacto varchar	50	no
        [Display(Name = "Nombre del contacto")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public required string NombreContactoFld { get; set; }

        //TelefonoContacto varchar	10	no
        [Display(Name = "Teléfono del contacto")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "El valor para {0} debe ser de {1} caracteres.")]
        [RegularExpression(RegExp.Numerico, ErrorMessage = "El valor para {0} es incorrecto.")]
        public required string TelefonoContactoFld { get; set; }

    }
}
