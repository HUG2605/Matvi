using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar seccion select desc grup.
    /// </summary>
public record ListarSeccionSelectDescGrupQry
        (string descripcionGrupo)
        : IRequest<List<BaseDto<byte>>>;
}
