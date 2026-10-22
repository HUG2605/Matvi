using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para detallar grupo.
    /// </summary>
public class DetallarGrupoDto : IIdDto<short>
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
        /// Texto descriptivo o detalle de ct secc nombre.
        /// </summary>
public string CtSecc_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Texto descriptivo o detalle de nombre.
        /// </summary>
public string Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Texto descriptivo o detalle de descripcion.
        /// </summary>
public string Descripcion { get; set; } = string.Empty;
    }
}
