using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Commands
{
    public record CerrarCorteCajaCmd(
        CerrarCorteCajaDto corte
    ) : IRequest<OperationResult<CerrarCorteCajaResDto>>;
}