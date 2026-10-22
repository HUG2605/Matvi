using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para crear grupo.
    /// </summary>
public class CrearGrupoDto : IIdDto<short>
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

        //IdCatSeccion    smallyint	 1	no
                /// <summary>
        /// Propiedad que representa ct secc id.
        /// </summary>
[Display(Name = "Sección")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        public required byte CtSecc_IdFld { get; set; }

        //Nombre varchar	50	no
                /// <summary>
        /// Texto descriptivo o detalle de nombre.
        /// </summary>
[Display(Name = "Nombre")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public required string NombreFld { get; set; }

        //Descripcion varchar	500	no
                /// <summary>
        /// Texto descriptivo o detalle de descripcion.
        /// </summary>
[Display(Name = "Descripción")]
        [Required(ErrorMessage = "El valor para {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El valor para {0} no debe exceder los {1} caracteres.")]
        public required string DescripcionFld { get; set; }
    }
}
