using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Commands
{
    public record EliminarGrupoCmd(
        short id
        ) : IRequest<OperationResult<bool>>;
}
