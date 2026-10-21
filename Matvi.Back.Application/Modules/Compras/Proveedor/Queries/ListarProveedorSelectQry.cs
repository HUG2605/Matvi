using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Compras.Proveedor.Queries
{
    public record ListarProveedorSelectQry
    : IRequest<List<BaseDto<int>>>;
}
