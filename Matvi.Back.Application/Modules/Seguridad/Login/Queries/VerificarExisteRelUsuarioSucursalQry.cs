using MediatR;

namespace Matvi.Back.Application.Modules.Seguridad.Login.Queries
{
    public record VerificarExisteRelUsuarioSucursalQry(
        short idUsuario,
        byte idSucursal
        ) : IRequest<bool>;
    
    
}
