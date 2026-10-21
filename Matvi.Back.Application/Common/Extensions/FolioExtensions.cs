using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;

namespace Matvi.Back.Application.Common.Extensions
{
    public static class FolioExtensions
    {
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
