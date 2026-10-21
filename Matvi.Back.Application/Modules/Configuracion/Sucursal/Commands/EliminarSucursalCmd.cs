using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Configuracion.Sucursal.Commands
{
    public record EliminarSucursalCmd(
        byte id
        ) : IRequest<OperationResult<bool>>;
    
}
