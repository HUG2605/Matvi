using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Compras.Proveedor.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Compras.Proveedor.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar proveedor grid.
    /// </summary>
public record ListarProveedorGridQry(
        FilterGrd filter
        ) : IRequest<DataGrd<ListarProveedorDto>>;
}
