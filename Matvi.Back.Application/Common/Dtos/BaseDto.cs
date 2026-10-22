namespace Matvi.Back.Application.Common.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para base.
    /// </summary>
public class BaseDto<TId>
    {
                /// <summary>
        /// Identificador único de id.
        /// </summary>
public TId Id { get; set; } = default!;

                /// <summary>
        /// Texto descriptivo o detalle de nombre.
        /// </summary>
public string Nombre { get; set; } = string.Empty;
    }
}
