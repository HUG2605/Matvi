using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.TipoGasto.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de crear tipo gasto.
    /// </summary>
public record CrearTipoGastoCmd(
          CrearTipoGastoDto tipoGasto
    ) : IRequest<OperationResult<bool>>;
}
