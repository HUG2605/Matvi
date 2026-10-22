using Matvi.Back.Application.Modules.Productos.Seccion.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de detallar seccion.
    /// </summary>
public record DetallarSeccionQry(
        byte id
        ) : IRequest<DetallarSeccionDto>;
    
}
