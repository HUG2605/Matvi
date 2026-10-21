using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.Seccion.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Commands
{
    public record EditarSeccionCmd (
        EditarSeccionDto seccion
        ) : IRequest<OperationResult<bool>>;    
}
