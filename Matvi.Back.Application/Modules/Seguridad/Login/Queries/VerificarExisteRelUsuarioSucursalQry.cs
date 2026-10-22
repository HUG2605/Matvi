using MediatR;

namespace Matvi.Back.Application.Modules.Seguridad.Login.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de verificar existe rel usuario sucursal.
    /// </summary>
public record VerificarExisteRelUsuarioSucursalQry(
        short idUsuario,
        byte idSucursal
        ) : IRequest<bool>;
    
    
}
