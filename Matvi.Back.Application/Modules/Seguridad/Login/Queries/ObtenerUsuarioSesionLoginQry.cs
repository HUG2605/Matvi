using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Seguridad.Login.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Seguridad.Login.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de obtener usuario sesion login.
    /// </summary>
public record ObtenerUsuarioSesionLoginQry(string Usuario, string Contrasenia)
        : IRequest<UsuarioLoginDto>;

}
