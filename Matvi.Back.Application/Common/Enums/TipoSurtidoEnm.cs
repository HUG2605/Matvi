using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Common.Enums
{
    /// <summary>
    /// Modalidad bajo la cual se surte un pedido desde el almacén.
    /// </summary>
    public enum TipoSurtidoEnm : byte
    {
        /// <summary>
        /// El pedido se entrega a un repartidor para que lo lleve al cliente.
        /// </summary>
        REPARTIDOR = 1,

        /// <summary>
        /// El pedido se entrega directamente al cliente desde el almacén
        /// </summary>
        DIRECTO = 2
    }
}
