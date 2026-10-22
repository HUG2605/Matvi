using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.TipoGasto.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de editar tipo gasto.
    /// </summary>
public record EditarTipoGastoQry(
        byte id
        ) : IRequest<EditarTipoGastoDto>;

}
