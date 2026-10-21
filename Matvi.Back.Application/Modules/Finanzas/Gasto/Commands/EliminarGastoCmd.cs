using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.Gasto.Commands
{
    public record EliminarGastoCmd(
        int id
        ) : IRequest<OperationResult<bool>>;
    
    
}
