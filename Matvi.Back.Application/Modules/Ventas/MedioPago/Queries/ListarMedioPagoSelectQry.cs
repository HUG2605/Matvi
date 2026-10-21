using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.MedioPago.Queries
{
    public record ListarMedioPagoSelectQry(
        ) : IRequest<List<BaseDto<byte>>>;
}
