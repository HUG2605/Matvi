using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Compras.Proveedor.Commands
{
    public  record EliminarProveedorCmd(
        int id
        ) : IRequest<OperationResult<bool>>;
}
