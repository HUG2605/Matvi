using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Compras.Proveedor.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar proveedor select.
    /// </summary>
public record ListarProveedorSelectQry
    : IRequest<List<BaseDto<int>>>;
}
