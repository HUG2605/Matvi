using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Commands
{
    public record EditarProductoSucursalCmd(
        EditarProductoSucursalDto productoSucursal
        ) : IRequest<OperationResult<bool>>;
}
