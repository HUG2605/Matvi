using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Compras.Compra.Commands;
using Matvi.Back.Application.Modules.Finanzas.Pago.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.Pago.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de crear pago.
    /// </summary>
public record CrearPagoCmd(
        CrearPagoDto pago
    ):IRequest<OperationResult<bool>>;
}
