using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Common.Enums
{
        /// <summary>
    /// Catálogo de opciones para rol.
    /// </summary>
public enum RolEnm : byte
    {
                /// <summary>
        /// Representa la opción system.
        /// </summary>
SYSTEM = 0,
                /// <summary>
        /// Representa la opción administrador.
        /// </summary>
ADMINISTRADOR = 1,
                /// <summary>
        /// Representa la opción vendedor.
        /// </summary>
VENDEDOR = 2,
                /// <summary>
        /// Representa la opción almacenista.
        /// </summary>
ALMACENISTA = 3,
                /// <summary>
        /// Representa la opción repartidor.
        /// </summary>
REPARTIDOR = 4
    }

}
