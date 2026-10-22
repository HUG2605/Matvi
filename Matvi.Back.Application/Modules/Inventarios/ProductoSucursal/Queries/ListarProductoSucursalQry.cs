using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar producto sucursal.
    /// </summary>
public record ListarProductoSucursalQry(
        byte idSucursal,
        FilterGrd filter
        ) : IRequest<DataGrd<ListarProductoSucursalDto>>;
}
