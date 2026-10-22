using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Compras.Proveedor.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de eliminar proveedor.
    /// </summary>
public  record EliminarProveedorCmd(
        int id
        ) : IRequest<OperationResult<bool>>;
}
