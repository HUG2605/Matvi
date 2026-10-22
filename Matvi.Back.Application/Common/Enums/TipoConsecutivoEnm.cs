using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Common.Enums
{
        /// <summary>
    /// Catálogo de opciones para tipo consecutivo.
    /// </summary>
public enum TipoConsecutivoEnm : byte
    {
                /// <summary>
        /// Representa la opción compra.
        /// </summary>
COMPRA = 1,
                /// <summary>
        /// Representa la opción lote.
        /// </summary>
LOTE = 2,
                /// <summary>
        /// Representa la opción movimiento de inventario.
        /// </summary>
MOVIMIENTO_DE_INVENTARIO = 3,
                /// <summary>
        /// Representa la opción pedido.
        /// </summary>
PEDIDO = 4,
                /// <summary>
        /// Representa la opción pago.
        /// </summary>
PAGO = 5,
                /// <summary>
        /// Representa la opción cobro.
        /// </summary>
COBRO = 6,
                /// <summary>
        /// Representa la opción gasto.
        /// </summary>
GASTO=7,
                /// <summary>
        /// Representa la opción corte de caja.
        /// </summary>
CORTE_DE_CAJA = 8

    }
}
