using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Infrastructure.Common.Services
{
    public interface IConsecutivoService
    {
        Task<int> GenerarNumeroAsync(
            byte idSucursal,
            byte idTipoConsecutivo,
            CancellationToken cancellationToken
            );
    }
}
