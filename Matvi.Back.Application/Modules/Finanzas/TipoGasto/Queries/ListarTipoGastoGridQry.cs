using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.TipoGasto.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar tipo gasto grid.
    /// </summary>
public record ListarTipoGastoGridQry(
        FilterGrd filter
        ) : IRequest<DataGrd<ListarTipoGastoDto>>;
}
