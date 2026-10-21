using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Commands
{
    public record EliminarPresentacionProductoCmd(
        int id
        ) : IRequest<OperationResult<bool>>;
}
