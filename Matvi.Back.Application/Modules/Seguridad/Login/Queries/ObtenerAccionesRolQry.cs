using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Seguridad.Login.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Seguridad.Login.Queries
{
    public record ObtenerAccionesRolQry(byte idRol)
        : IRequest<List<AccionLoginDto>>;
    
}
