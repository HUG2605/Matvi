using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.Cobro.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.Cobro.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de crear cobro.
    /// </summary>
public record CrearCobroCmd(
        CrearCobroDto cobro
    ) : IRequest<OperationResult<bool>>;
}