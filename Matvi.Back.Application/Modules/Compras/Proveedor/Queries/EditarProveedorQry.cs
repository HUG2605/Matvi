using Matvi.Back.Application.Modules.Compras.Proveedor.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Compras.Proveedor.Queries
{
    public record EditarProveedorQry(
        int id
        ) : IRequest<EditarProveedorDto>;

}
