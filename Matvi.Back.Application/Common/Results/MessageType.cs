using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Common.Results
{
        /// <summary>
    /// Catálogo de opciones para message type.
    /// </summary>
public enum MessageType
    {
                /// <summary>
        /// Representa la opción success.
        /// </summary>
Success = 1,
                /// <summary>
        /// Representa la opción info.
        /// </summary>
Info = 2,
                /// <summary>
        /// Representa la opción warning.
        /// </summary>
Warning = 3,
                /// <summary>
        /// Representa la opción error.
        /// </summary>
Error = 4
    }
}
