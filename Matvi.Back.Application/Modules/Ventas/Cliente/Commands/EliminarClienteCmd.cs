using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Commands
{
    public record EliminarClienteCmd(
        int id
        ) : IRequest<OperationResult<bool>>;
}
