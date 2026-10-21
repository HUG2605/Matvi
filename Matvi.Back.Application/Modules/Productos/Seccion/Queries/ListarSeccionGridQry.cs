using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Productos.Seccion.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Queries
{
    public record ListarSeccionGridQry(
        FilterGrd filter
        ) : IRequest<DataGrd<ListarSeccionDto>>;

}