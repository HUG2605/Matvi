using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Common.Grid
{
        /// <summary>
    /// Representa la clase para data grd.
    /// </summary>
public class DataGrd<T>
    {
                /// <summary>
        /// Monto económico correspondiente a total items.
        /// </summary>
public int TotalItems { get; set; }
                /// <summary>
        /// Propiedad que representa items.
        /// </summary>
public IReadOnlyList<T> Items { get; set; } = [];
    }

}
