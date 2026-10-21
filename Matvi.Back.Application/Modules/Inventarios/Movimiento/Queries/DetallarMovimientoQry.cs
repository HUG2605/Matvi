using Matvi.Back.Application.Modules.Inventarios.Movimiento.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Inventarios.Movimiento.Queries
{
    public record DetallarMovimientoQry(
        int id
        ) : IRequest<DetalleMovimientoDto>;

}