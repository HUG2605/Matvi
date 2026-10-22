using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Productos.Grupo.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar grupos.
    /// </summary>
public record ListarGruposQry(
        FilterGrd filter
        ) : IRequest<DataGrd<ListarGrupoDto>>;
}
