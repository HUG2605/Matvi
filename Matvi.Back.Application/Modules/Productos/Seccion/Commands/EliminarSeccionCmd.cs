using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Commands
{
    public record EliminarSeccionCmd(
        byte id
        ) : IRequest<OperationResult<bool>>;
}
