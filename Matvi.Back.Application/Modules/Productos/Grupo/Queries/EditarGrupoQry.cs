using Matvi.Back.Application.Modules.Productos.Grupo.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de editar grupo.
    /// </summary>
public record EditarGrupoQry(
        short id
        ) : IRequest<EditarGrupoDto>;
}
