using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para editar usuario.
    /// </summary>
public class EditarUsuarioDto : CrearUsuarioDto
    {
                /// <summary>
        /// Propiedad que representa activo.
        /// </summary>
[Display(Name = "Activo")]
        public bool ActivoFld { get; set; }

                /// <summary>
        /// Propiedad que representa reiniciar contrasenia.
        /// </summary>
[Display(Name = "Reiniciar contraseña")]
        public bool ReiniciarContraseniaFld { get; set; }

    }
}
