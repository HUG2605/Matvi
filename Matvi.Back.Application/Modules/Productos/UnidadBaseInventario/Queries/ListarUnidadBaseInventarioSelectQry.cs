using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.UnidadBaseInventario.Queries
{
    public record ListarUnidadBaseInventarioSelectQry(
        ): IRequest<List<BaseDto<byte>>>;

}
