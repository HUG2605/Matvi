using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para listar usuario.
    /// </summary>
public class ListarUsuarioDto : IIdDto<short>
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
        /// Texto descriptivo o detalle de ct rol nombre.
        /// </summary>
public string CtRol_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Texto descriptivo o detalle de nombre completo.
        /// </summary>
public string NombreCompleto { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa usuario.
        /// </summary>
public string Usuario { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa telefono.
        /// </summary>
public string Telefono { get; set; } = string.Empty;
                /// <summary>
        /// Fecha y hora correspondiente a fecha ultimo acceso.
        /// </summary>
public DateTime? FechaUltimoAcceso { get; set; }
                /// <summary>
        /// Indica si activo se encuentra activo o habilitado.
        /// </summary>
public bool Activo { get; set; }

    }
}
