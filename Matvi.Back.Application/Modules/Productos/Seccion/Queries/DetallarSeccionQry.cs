using Matvi.Back.Application.Modules.Productos.Seccion.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Queries
{
    public record DetallarSeccionQry(
        byte id
        ) : IRequest<DetallarSeccionDto>;
    
}
