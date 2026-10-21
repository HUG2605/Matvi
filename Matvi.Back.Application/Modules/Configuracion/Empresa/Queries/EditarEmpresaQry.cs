using Matvi.Back.Application.Modules.Configuracion.Empresa.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Configuracion.Empresa.Queries
{
    public record EditarEmpresaQry(
        byte id
        ): IRequest<EditarEmpresaDto>;
    
}
