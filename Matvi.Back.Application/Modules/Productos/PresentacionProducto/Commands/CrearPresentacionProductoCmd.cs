using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Commands
{
    public record CrearPresentacionProductoCmd(
        CrearPresentacionProductoDto presentacionProducto
          ) : IRequest<OperationResult<bool>>;
}
