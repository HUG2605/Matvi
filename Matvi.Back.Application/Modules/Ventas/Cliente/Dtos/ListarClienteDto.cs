using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para listar cliente.
    /// </summary>
public class ListarClienteDto : IIdDto<int>
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
        /// Propiedad que representa alias.
        /// </summary>
public string? Alias { get; set; }
                /// <summary>
        /// Propiedad que representa direccion.
        /// </summary>
public string Direccion { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa telefono.
        /// </summary>
public string Telefono { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa limite credito semanal.
        /// </summary>
public decimal LimiteCreditoSemanal { get; set; }
    }
}
