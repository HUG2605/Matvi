using Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de detallar movimiento bancario.
    /// </summary>
public record DetallarMovimientoBancarioQry(int Id) : IRequest<DetallarMovimientoBancarioDto?>;
}
