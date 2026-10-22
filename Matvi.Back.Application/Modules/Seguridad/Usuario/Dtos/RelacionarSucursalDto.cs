using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para relacionar sucursal.
    /// </summary>
public class RelacionarSucursalDto : IIdDto<byte>
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

                /// <summary>
        /// Texto descriptivo o detalle de nombre.
        /// </summary>
public string Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa direccion.
        /// </summary>
public string Direccion { get; set; } = string.Empty;
                /// <summary>
        /// Indica si esta relacionada se encuentra activo o habilitado.
        /// </summary>
public bool EstaRelacionada { get; set; }
    }
}
