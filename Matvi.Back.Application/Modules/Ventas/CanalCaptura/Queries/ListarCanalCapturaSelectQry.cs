using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.CanalCaptura.Queries
{
    public record ListarCanalCapturaSelectQry(
        ) : IRequest<List<BaseDto<byte>>>;
}
