using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Ventas.Cliente.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Queries
{
    public record ListarClienteQry(
        FilterGrd filter
        ) : IRequest<DataGrd<ListarClienteDto>>;
}
