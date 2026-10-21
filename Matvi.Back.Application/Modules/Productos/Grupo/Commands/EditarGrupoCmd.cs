using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.Grupo.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Commands
{
    public record EditarGrupoCmd (
        EditarGrupoDto grupo
        ) : IRequest<OperationResult<bool>>;
}
