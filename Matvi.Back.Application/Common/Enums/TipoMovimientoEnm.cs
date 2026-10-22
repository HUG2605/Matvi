using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Common.Enums
{
        /// <summary>
    /// Catálogo de opciones para tipo movimiento.
    /// </summary>
public enum TipoMovimientoEnm
    {

                /// <summary>
        /// Representa la opción ent compra.
        /// </summary>
ENT_COMPRA = 1,
                /// <summary>
        /// Representa la opción sal venta.
        /// </summary>
SAL_VENTA = 2,
                /// <summary>
        /// Representa la opción ent reingreso.
        /// </summary>
ENT_REINGRESO = 3,
                /// <summary>
        /// Representa la opción ent traspaso de entrada.
        /// </summary>
ENT_TRASPASO_DE_ENTRADA = 4,
                /// <summary>
        /// Representa la opción sal traspaso de salida.
        /// </summary>
SAL_TRASPASO_DE_SALIDA = 5,
                /// <summary>
        /// Representa la opción ent ajuste positivo.
        /// </summary>
ENT_AJUSTE_POSITIVO = 6,
                /// <summary>
        /// Representa la opción sal ajuste negativo.
        /// </summary>
SAL_AJUSTE_NEGATIVO = 7

    }
}
