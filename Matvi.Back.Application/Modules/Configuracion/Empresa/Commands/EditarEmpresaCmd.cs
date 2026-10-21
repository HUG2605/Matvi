using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Configuracion.Empresa.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Configuracion.Empresa.Commands
{
    public record EditarEmpresaCmd(
        EditarEmpresaDto empresa
        ) : IRequest<OperationResult<bool>>;

}
