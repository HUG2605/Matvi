using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Queries
{
    public record ListarSeccionSelectDescGrupQry
        (string descripcionGrupo)
        : IRequest<List<BaseDto<byte>>>;
}
