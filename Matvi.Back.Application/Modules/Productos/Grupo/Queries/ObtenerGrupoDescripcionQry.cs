using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de obtener grupo descripcion.
    /// </summary>
public record ObtenerGrupoDescripcionQry(
        short id
        ) : IRequest<string>;
}
