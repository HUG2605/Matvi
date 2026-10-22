using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar cliente select coincidencia.
    /// </summary>
public record ListarClienteSelectCoincidenciaQry(
        string coincidencia
        ) : IRequest<List<BaseDto<int>>>;
}
