using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Queries
{
    public record ListarGrupoSelectQry (
        byte idCatSeccion, string descripcion)
        : IRequest<List<BaseDto<short>>>;
}
