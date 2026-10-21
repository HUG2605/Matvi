using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Queries
{
    public record ListarRepartidorSelectQry(
        string coincidencia
        ) : IRequest<List<BaseDto<short>>>;
}