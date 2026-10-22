using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.UnidadBaseInventario.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar unidad base inventario select.
    /// </summary>
public record ListarUnidadBaseInventarioSelectQry(
        ): IRequest<List<BaseDto<byte>>>;

}
