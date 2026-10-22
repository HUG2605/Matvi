using Matvi.Back.Application.Modules.Seguridad.Usuario.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de editar usuario.
    /// </summary>
public record EditarUsuarioQry (
        short id
        ) : IRequest<EditarUsuarioDto>;
    
}
