namespace Matvi.Front.UI.Dto
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para data table response.
    /// </summary>
public class DataTableResponseDto<T>
    {
                /// <summary>
        /// Propiedad que representa draw.
        /// </summary>
public int Draw { get; set; }
                /// <summary>
        /// Monto económico correspondiente a records total.
        /// </summary>
public int RecordsTotal { get; set; }
                /// <summary>
        /// Propiedad que representa records filtered.
        /// </summary>
public int RecordsFiltered { get; set; }
                /// <summary>
        /// Propiedad que representa data.
        /// </summary>
public IReadOnlyList<T> Data { get; set; } = [];
    }

}
