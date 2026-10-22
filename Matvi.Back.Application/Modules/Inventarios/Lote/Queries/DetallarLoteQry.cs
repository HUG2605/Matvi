using Matvi.Back.Application.Modules.Inventarios.Lote.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Inventarios.Lote.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de detallar lote.
    /// </summary>
public record DetallarLoteQry(
        int id
        ) : IRequest<DetallarLoteDto>;
 
}
