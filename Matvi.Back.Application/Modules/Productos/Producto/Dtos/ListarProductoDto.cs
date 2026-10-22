using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Productos.Producto.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para listar producto.
    /// </summary>
public class ListarProductoDto : IIdDto<int>
    {
        #region Implementacion de IIdDto

                /// <summary>
        /// Identificador único de id.
        /// </summary>
public int Id { get; set; }
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
        /// Indica si especificacion tecnica se encuentra activo o habilitado.
        /// </summary>
public string? EspecificacionTecnica { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa marca.
        /// </summary>
public string? Marca { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de ct secc nombre.
        /// </summary>
public string CtSecc_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Texto descriptivo o detalle de ct grup nombre.
        /// </summary>
public string CtGrup_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Indica si activo se encuentra activo o habilitado.
        /// </summary>
public bool Activo { get; set; }
    }
}
