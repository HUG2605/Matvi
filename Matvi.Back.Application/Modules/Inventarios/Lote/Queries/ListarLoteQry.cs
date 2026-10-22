using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Inventarios.Lote.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Inventarios.Lote.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar lote.
    /// </summary>
public record ListarLoteQry(
        byte idSucursal,
        FilterGrd filter
        ) : IRequest<DataGrd<ListarLoteDto>>;
}
