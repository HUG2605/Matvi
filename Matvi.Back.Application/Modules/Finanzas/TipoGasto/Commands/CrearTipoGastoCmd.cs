using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.TipoGasto.Commands
{
    public record CrearTipoGastoCmd(
          CrearTipoGastoDto tipoGasto
    ) : IRequest<OperationResult<bool>>;
}
