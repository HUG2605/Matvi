using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Productos.Seccion.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar seccion grid.
    /// </summary>
public record ListarSeccionGridQry(
        FilterGrd filter
        ) : IRequest<DataGrd<ListarSeccionDto>>;

}