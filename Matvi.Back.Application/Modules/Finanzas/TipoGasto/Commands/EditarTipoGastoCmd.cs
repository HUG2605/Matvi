using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.TipoGasto.Commands
{
    public record EditarTipoGastoCmd(
             EditarTipoGastoDto tipoGasto
        ) : IRequest<OperationResult<bool>>;
}
