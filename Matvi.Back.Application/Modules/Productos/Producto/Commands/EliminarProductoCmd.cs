using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Producto.Commands
{
    public record EliminarProductoCmd(
        int id
        ) : IRequest<OperationResult<bool>>;
}
