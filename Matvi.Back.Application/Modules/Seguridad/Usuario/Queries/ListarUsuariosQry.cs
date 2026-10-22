using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar usuarios.
    /// </summary>
public record ListarUsuariosQry(
        FilterGrd filter
        ) : IRequest<DataGrd<ListarUsuarioDto>>;
}
