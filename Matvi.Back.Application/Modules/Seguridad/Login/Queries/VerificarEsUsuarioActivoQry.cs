using MediatR;

namespace Matvi.Back.Application.Modules.Seguridad.Login.Queries
{
    public record VerificarEsUsuarioActivoQry(
        short id
        ) : IRequest<bool>;
}
