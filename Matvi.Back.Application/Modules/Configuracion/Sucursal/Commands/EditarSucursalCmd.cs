using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Configuracion.Sucursal.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Configuracion.Sucursal.Commands
{
    public record EditarSucursalCmd (
             EditarSucursalDto sucursal
        ) : IRequest<OperationResult<bool>>;

}
