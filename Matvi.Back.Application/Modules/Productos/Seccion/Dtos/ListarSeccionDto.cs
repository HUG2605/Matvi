using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para listar seccion.
    /// </summary>
public class ListarSeccionDto : IIdDto<byte>
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
    }
}
