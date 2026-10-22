using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Infrastructure.Common.Services
{
    /// <summary>
    /// Servicio para la generación atómica y concurrente de números consecutivos por sucursal y tipo de operación.
    /// </summary>
    public interface IConsecutivoService
    {
        /// <summary>
        /// Genera de manera atómica el siguiente número consecutivo para una sucursal y tipo de documento específico.
        /// </summary>
        /// <param name="idSucursal">Identificador de la sucursal.</param>
        /// <param name="idTipoConsecutivo">Tipo de consecutivo a incrementar (compra, lote, pedido, etc.).</param>
        /// <param name="cancellationToken">Token de cancelación de la operación asíncrona.</param>
        /// <returns>El nuevo número consecutivo asignado.</returns>
        Task<int> GenerarNumeroAsync(
            byte idSucursal,
            byte idTipoConsecutivo,
            CancellationToken cancellationToken
            );
    }
}
