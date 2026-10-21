using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.Seccion.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Commands
{
    public record CrearSeccionCmd(
        CrearSeccionDto seccion
    ) : IRequest<OperationResult<bool>>;
}
