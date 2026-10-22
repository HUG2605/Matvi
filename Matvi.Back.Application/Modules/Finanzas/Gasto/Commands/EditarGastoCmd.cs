using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Dtos;
using Matvi.Back.Infrastructure.Common.Attributes;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.Gasto.Commands
{

        /// <summary>
    /// Comando CQRS para la operación de editar gasto.
    /// </summary>
public record EditarGastoCmd(
        EditarGastoDto gasto
        ) : IRequest<OperationResult<bool>>;

  
}
