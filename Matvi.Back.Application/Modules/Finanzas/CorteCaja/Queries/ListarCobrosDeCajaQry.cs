using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar cobros de caja.
    /// </summary>
public record ListarCobrosDeCajaQry(
        int IdOpCorteCaja,
        FilterGrd Filter
    ) : IRequest<DataGrd<ListarCobroCajaDto>>;
}
