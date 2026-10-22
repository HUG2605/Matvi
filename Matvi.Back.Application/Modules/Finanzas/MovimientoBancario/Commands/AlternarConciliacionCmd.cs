using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de alternar conciliacion.
    /// </summary>
public record AlternarConciliacionCmd(
        int Id,
        bool? ForzarEstado = null
    ) : IRequest<OperationResult<bool>>;
}
