using Matvi.Back.Application.Modules.Productos.Grupo.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de detallar grupo.
    /// </summary>
public record DetallarGrupoQry(
        short id
        ) : IRequest<DetallarGrupoDto>;
}
