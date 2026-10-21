using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.TipoGasto.Queries
{
    public record DetallarTipoGastoQry(
        byte id
        ) : IRequest<DetallarTipoGastoDto>;
}
