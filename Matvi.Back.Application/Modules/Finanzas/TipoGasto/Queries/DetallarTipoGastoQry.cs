using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.TipoGasto.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de detallar tipo gasto.
    /// </summary>
public record DetallarTipoGastoQry(
        byte id
        ) : IRequest<DetallarTipoGastoDto>;
}
