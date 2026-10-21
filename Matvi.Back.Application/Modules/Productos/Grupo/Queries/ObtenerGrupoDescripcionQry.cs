using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Queries
{
    public record ObtenerGrupoDescripcionQry(
        short id
        ) : IRequest<string>;
}
