using Matvi.Back.Application.Common.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Compras.Proveedor.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar proveedor select coincidencia.
    /// </summary>
public record ListarProveedorSelectCoincidenciaQry(
        string coincidencia
    ) : IRequest<List<BaseDto<int>>>;
}