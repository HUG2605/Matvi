using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Producto.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar producto select coincidencia.
    /// </summary>
public record ListarProductoSelectCoincidenciaQry(
        string coincidencia
        ) : IRequest<List<BaseDto<int>>>;
}
