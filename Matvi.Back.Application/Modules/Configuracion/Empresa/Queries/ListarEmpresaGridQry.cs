using Matvi.Back.Application.Modules.Configuracion.Empresa.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Configuracion.Empresa.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar empresa grid.
    /// </summary>
public record ListarEmpresaGridQry 
        : IRequest<List<ListarEmpresaDto>>;

}
