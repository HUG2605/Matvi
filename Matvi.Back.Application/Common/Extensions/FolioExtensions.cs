using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;

namespace Matvi.Back.Application.Common.Extensions
{
        /// <summary>
    /// Representa la clase para folio extensions.
    /// </summary>
public static class FolioExtensions
    {
                /// <summary>
        /// Recupera la información correspondiente a get folio.
        /// </summary>
        /// <param name="dto">Parámetro dto.</param>
        /// <param name="tipoConsecutivo">Parámetro tipo consecutivo.</param>
        /// <returns>Objeto con la información resultante de get folio.</returns>
public static string GetFolio(this IFolioDto dto, TipoConsecutivoEnm tipoConsecutivo)
        {
            string prefijo = tipoConsecutivo switch
            {
                TipoConsecutivoEnm.COMPRA => "COM",
                TipoConsecutivoEnm.LOTE => "LOT",
                TipoConsecutivoEnm.MOVIMIENTO_DE_INVENTARIO => "MOV",
                TipoConsecutivoEnm.PEDIDO => "PED",
                TipoConsecutivoEnm.PAGO => "PAG",
                TipoConsecutivoEnm.COBRO => "COB",
                TipoConsecutivoEnm.GASTO => "GAS",
                TipoConsecutivoEnm.CORTE_DE_CAJA => "CRT",
                _ => ""

            };

            string numero = dto.Numero.ToString().PadLeft(5, '0');

            return $"{dto.CtSucu_Clave}{prefijo}{numero}";
        }
    }
}
