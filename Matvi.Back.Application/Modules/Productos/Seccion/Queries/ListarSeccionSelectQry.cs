using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Queries
{
    public record ListarSeccionSelectQry
        : IRequest<List<BaseDto<byte>>>;
}
