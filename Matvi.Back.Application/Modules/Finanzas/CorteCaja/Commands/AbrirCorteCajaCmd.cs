using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de abrir corte caja.
    /// </summary>
public record AbrirCorteCajaCmd(
        AbrirCorteCajaDto corte
    ) : IRequest<OperationResult<bool>>;
}