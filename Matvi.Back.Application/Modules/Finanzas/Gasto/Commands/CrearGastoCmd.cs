using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.Gasto.Commands
{
    public record CrearGastoCmd(CrearGastoDto gasto)
        : IRequest<OperationResult<bool>>;
}