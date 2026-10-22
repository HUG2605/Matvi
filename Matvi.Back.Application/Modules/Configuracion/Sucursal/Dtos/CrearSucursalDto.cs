using Matvi.Back.Application.Common.Constants;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Configuracion.Sucursal.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para crear sucursal.
    /// </summary>
public class CrearSucursalDto : IIdDto<byte>
    {
        #region Implementacion de IIdDto

                /// <summary>
        /// Identificador único de id.
        /// </summary>
public byte Id { get; set; }
                /// <summary>
        /// Identificador único de safe id.
        /// </summary>
public string SafeId => Id.GetSafeId();

        #endregion

        //Nombre varchar	20	no
                /// <summary>
        /// Texto descriptivo o detalle de nombre.
        /// </summary>
[Display(Name = "Nombre")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(20, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public required string NombreFld { get; set; }
        
                /// <summary>
        /// Propiedad que representa clave.
        /// </summary>
[Display(Name = "Clave")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "El valor para {0} debe ser de {1} caracteres.")]
        public required string ClaveFld { get; set; }

        //Calle	varchar	250	no
                /// <summary>
        /// Propiedad que representa direccion.
        /// </summary>
[Display(Name = "Dirección")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(250, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public required string DireccionFld { get; set; }

        //Telefono	varchar	10	no
                /// <summary>
        /// Propiedad que representa telefono.
        /// </summary>
[Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "El valor para {0} debe ser de {1} caracteres.")]
        [RegularExpression(RegExp.Numerico, ErrorMessage = "El valor para {0} es incorrecto.")]
        public required string TelefonoFld { get; set; }

    }
}
