using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.CanalCaptura.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar canal captura select.
    /// </summary>
public record ListarCanalCapturaSelectQry(
        ) : IRequest<List<BaseDto<byte>>>;
}
