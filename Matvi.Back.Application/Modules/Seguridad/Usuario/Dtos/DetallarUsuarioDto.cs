using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para detallar usuario.
    /// </summary>
public class DetallarUsuarioDto : IIdDto<short>
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
        /// Texto descriptivo o detalle de nombre.
        /// </summary>
public string Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa primer apellido.
        /// </summary>
public string PrimerApellido { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa segundo apellido.
        /// </summary>
public string SegundoApellido { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa direccion.
        /// </summary>
public string Direccion { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa telefono.
        /// </summary>
public string Telefono { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa usuario.
        /// </summary>
public string Usuario { get; set; } = string.Empty;
                /// <summary>
        /// Indica si activo se encuentra activo o habilitado.
        /// </summary>
public bool Activo { get; set; }
                /// <summary>
        /// Colección de elementos asociados a sucursales.
        /// </summary>
public List<RelacionarSucursalDto>? Sucursales { get; set; }

    }
}
