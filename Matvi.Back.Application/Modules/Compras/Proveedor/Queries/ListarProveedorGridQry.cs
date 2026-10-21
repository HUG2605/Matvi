using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Compras.Proveedor.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Compras.Proveedor.Queries
{
    public record ListarProveedorGridQry(
        FilterGrd filter
        ) : IRequest<DataGrd<ListarProveedorDto>>;
}
