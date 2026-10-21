using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.TipoGasto.Queries
{
    public record EditarTipoGastoQry(
        byte id
        ) : IRequest<EditarTipoGastoDto>;

}
