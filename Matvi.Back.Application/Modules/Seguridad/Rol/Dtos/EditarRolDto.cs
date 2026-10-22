using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Back.Application.Modules.Seguridad.Rol.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para editar rol.
    /// </summary>
public class EditarRolDto : IIdDto<short>
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
        /// Texto descriptivo o detalle de nombre.
        /// </summary>
[Display(Name = "Nombre")]
        public string NombreFld { get; set; } = string.Empty;

                /// <summary>
        /// Colección de elementos asociados a funciones.
        /// </summary>
public List<RolFuncionDto>? Funciones { get; set; }

    }
}
