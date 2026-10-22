using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.Grupo.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de editar grupo.
    /// </summary>
public record EditarGrupoCmd (
        EditarGrupoDto grupo
        ) : IRequest<OperationResult<bool>>;
}
