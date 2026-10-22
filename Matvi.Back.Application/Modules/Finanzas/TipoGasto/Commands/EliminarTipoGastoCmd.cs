using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.TipoGasto.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de eliminar tipo gasto.
    /// </summary>
public record EliminarTipoGastoCmd(
        byte id
        ) : IRequest<OperationResult<bool>>;
}
