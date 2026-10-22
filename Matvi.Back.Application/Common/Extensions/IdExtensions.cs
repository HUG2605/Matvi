using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Common.Extensions
{
        /// <summary>
    /// Representa la clase para id extensions.
    /// </summary>
public static class IdExtensions
    {
                /// <summary>
        /// Recupera la información correspondiente a get safe id.
        /// </summary>
        /// <param name="id">Identificador único del registro u objeto.</param>
        /// <returns>Objeto con la información resultante de get safe id.</returns>
public static string GetSafeId<TId>(this TId id)
            => id?.ToString()?.ToObfuscated() ?? string.Empty;
    }
}
