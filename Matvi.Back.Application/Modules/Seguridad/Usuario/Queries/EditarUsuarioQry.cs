using Matvi.Back.Application.Modules.Seguridad.Usuario.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Queries
{
    public record EditarUsuarioQry (
        short id
        ) : IRequest<EditarUsuarioDto>;
    
}
