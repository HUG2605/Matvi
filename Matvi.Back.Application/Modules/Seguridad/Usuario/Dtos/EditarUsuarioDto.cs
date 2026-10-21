using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Dtos
{
    public class EditarUsuarioDto : CrearUsuarioDto
    {
        [Display(Name = "Activo")]
        public bool ActivoFld { get; set; }

        [Display(Name = "Reiniciar contraseña")]
        public bool ReiniciarContraseniaFld { get; set; }

    }
}
