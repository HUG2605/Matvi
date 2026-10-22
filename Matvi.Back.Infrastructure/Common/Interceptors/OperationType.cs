using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Infrastructure.Common.Interceptors
{
    /// <summary>
    /// Tipo de operación realizada sobre una entidad durante el proceso de auditoría.
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// Inserción de una nueva entidad.
        /// </summary>
        INSERT = 1,
        /// <summary>
        /// Modificación de una entidad existente.
        /// </summary>
        UPDATE = 2,
        /// <summary>
        /// Eliminación de una entidad.
        /// </summary>
        DELETE = 3
    }
}
