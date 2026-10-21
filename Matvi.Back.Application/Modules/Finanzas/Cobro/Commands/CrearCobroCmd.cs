using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.Cobro.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.Cobro.Commands
{
    public record CrearCobroCmd(
        CrearCobroDto cobro
    ) : IRequest<OperationResult<bool>>;
}