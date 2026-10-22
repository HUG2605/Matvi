using Matvi.Back.Application.Modules.Inventarios.Lote.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Inventarios.Lote.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de regularizar lote.
    /// </summary>
public record RegularizarLoteQry(
        int id
        ) : IRequest<RegularizarLoteDto>;
}
