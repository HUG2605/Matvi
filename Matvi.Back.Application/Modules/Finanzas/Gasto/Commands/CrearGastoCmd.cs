using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.Gasto.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de crear gasto.
    /// </summary>
public record CrearGastoCmd(CrearGastoDto gasto)
        : IRequest<OperationResult<bool>>;
}