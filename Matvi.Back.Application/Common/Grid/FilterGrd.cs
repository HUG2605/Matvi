namespace Matvi.Back.Application.Common.Grid
{
        /// <summary>
    /// Representa la clase para filter grd.
    /// </summary>
public class FilterGrd
    {
                /// <summary>
        /// Propiedad que representa search.
        /// </summary>
public string? Search { get; set; }
                /// <summary>
        /// Propiedad que representa page.
        /// </summary>
public int Page { get; set; } = 1;
                /// <summary>
        /// Propiedad que representa page size.
        /// </summary>
public int PageSize { get; set; } = 20;
                /// <summary>
        /// Propiedad que representa sort by.
        /// </summary>
public string? SortBy { get; set; }
                /// <summary>
        /// Propiedad que representa sort desc.
        /// </summary>
public bool SortDesc { get; set; }
    }
}
