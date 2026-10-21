using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Inventarios.Lote.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Inventarios.Lote.Commands
{
    public record RegularizarLoteCmd(
        RegularizarLoteDto Lote
        ) : IRequest<OperationResult<bool>>;
}
