using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Configuracion.Sucursal.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para detallar sucursal.
    /// </summary>
public class DetallarSucursalDto : IIdDto<byte>
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
        /// Propiedad que representa clave.
        /// </summary>
public string Clave { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa direcci n.
        /// </summary>
public string Dirección { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa telefono.
        /// </summary>
public string Telefono { get; set; } = string.Empty;

    }
}
