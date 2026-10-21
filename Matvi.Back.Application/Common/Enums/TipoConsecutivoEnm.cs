using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Common.Enums
{
    public enum TipoConsecutivoEnm : byte
    {
        COMPRA = 1,
        LOTE = 2,
        MOVIMIENTO_DE_INVENTARIO = 3,
        PEDIDO = 4,
        PAGO = 5,
        COBRO = 6,
        GASTO=7,
        CORTE_DE_CAJA = 8

    }
}
