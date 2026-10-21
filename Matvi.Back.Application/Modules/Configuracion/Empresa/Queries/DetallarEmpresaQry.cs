using Matvi.Back.Application.Modules.Configuracion.Empresa.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Configuracion.Empresa.Queries
{
    public record DetallarEmpresaQry(
        byte id
        ) : IRequest<DetallarEmpresaDto>;
    
}
