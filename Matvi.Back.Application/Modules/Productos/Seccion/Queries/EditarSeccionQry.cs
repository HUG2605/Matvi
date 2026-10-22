using Matvi.Back.Application.Modules.Productos.Seccion.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de editar seccion.
    /// </summary>
public record EditarSeccionQry(
        byte id
        ) : IRequest<EditarSeccionDto>;
}
