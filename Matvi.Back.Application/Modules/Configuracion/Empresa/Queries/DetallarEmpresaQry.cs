using Matvi.Back.Application.Modules.Configuracion.Empresa.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Configuracion.Empresa.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de detallar empresa.
    /// </summary>
public record DetallarEmpresaQry(
        byte id
        ) : IRequest<DetallarEmpresaDto>;
    
}
