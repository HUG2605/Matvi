using Matvi.Back.Application.Modules.Configuracion.Empresa.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Configuracion.Empresa.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de editar empresa.
    /// </summary>
public record EditarEmpresaQry(
        byte id
        ): IRequest<EditarEmpresaDto>;
    
}
