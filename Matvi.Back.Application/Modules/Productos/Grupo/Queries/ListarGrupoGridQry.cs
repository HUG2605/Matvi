using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Productos.Grupo.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Queries
{
    public record ListarGruposQry(
        FilterGrd filter
        ) : IRequest<DataGrd<ListarGrupoDto>>;
}
