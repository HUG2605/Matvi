using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Commands
{
    public record AlternarConciliacionCmd(
        int Id,
        bool? ForzarEstado = null
    ) : IRequest<OperationResult<bool>>;
}
