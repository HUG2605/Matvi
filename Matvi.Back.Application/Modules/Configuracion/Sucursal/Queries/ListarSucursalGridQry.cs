using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Configuracion.Sucursal.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Configuracion.Sucursal.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar sucursal grid.
    /// </summary>
public record ListarSucursalGridQry(
        FilterGrd filter
        ) : IRequest<DataGrd<ListarSucursalDto>>;
}
