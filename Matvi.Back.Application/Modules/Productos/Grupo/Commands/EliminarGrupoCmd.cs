using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de eliminar grupo.
    /// </summary>
public record EliminarGrupoCmd(
        short id
        ) : IRequest<OperationResult<bool>>;
}
