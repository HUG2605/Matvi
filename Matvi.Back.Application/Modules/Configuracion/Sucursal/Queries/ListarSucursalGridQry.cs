using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Configuracion.Sucursal.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Configuracion.Sucursal.Queries
{
    public record ListarSucursalGridQry(
        FilterGrd filter
        ) : IRequest<DataGrd<ListarSucursalDto>>;
}
