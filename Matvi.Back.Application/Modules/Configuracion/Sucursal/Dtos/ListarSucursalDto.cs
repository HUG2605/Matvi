using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Configuracion.Sucursal.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para listar sucursal.
    /// </summary>
public class ListarSucursalDto : IIdDto<byte>
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
        /// Propiedad que representa direccion.
        /// </summary>
public string Direccion { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa telefono.
        /// </summary>
public string Telefono { get; set; } = string.Empty;

    }
}
