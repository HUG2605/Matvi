using MediatR;

namespace Matvi.Back.Application.Modules.Seguridad.Login.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de verificar es usuario activo.
    /// </summary>
public record VerificarEsUsuarioActivoQry(
        short id
        ) : IRequest<bool>;
}
