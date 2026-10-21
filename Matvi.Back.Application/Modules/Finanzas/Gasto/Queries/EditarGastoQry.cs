using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.Gasto.Queries
{
    public record EditarGastoQry(
        int id
        ) : IRequest<EditarGastoDto>;
    
    
}
