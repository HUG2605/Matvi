using Matvi.Back.Application.Modules.Productos.Grupo.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Queries
{
    public record EditarGrupoQry(
        short id
        ) : IRequest<EditarGrupoDto>;
}
