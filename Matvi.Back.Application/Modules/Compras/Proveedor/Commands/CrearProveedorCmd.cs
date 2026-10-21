using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Compras.Proveedor.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Compras.Proveedor.Commands
{
    public record CrearProveedorCmd(
          CrearProveedorDto proveedor
    ) : IRequest<OperationResult<bool>>;
}
