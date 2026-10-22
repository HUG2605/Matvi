using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar repartidor select.
    /// </summary>
public record ListarRepartidorSelectQry(
        string coincidencia
        ) : IRequest<List<BaseDto<short>>>;
}