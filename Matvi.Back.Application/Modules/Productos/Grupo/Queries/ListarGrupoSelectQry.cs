using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar grupo select.
    /// </summary>
public record ListarGrupoSelectQry (
        byte idCatSeccion, string descripcion)
        : IRequest<List<BaseDto<short>>>;
}
