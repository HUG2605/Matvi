using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.TipoGasto.Queries
{
    public record ListarTipoGastoGridQry(
        FilterGrd filter
        ) : IRequest<DataGrd<ListarTipoGastoDto>>;
}
