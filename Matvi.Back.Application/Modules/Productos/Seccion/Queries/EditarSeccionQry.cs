using Matvi.Back.Application.Modules.Productos.Seccion.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Queries
{
    public record EditarSeccionQry(
        byte id
        ) : IRequest<EditarSeccionDto>;
}
