using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar seccion select.
    /// </summary>
public record ListarSeccionSelectQry
        : IRequest<List<BaseDto<byte>>>;
}
