using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Finanzas.Cobro.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.Cobro.Queries
{
    public record ListarCobroQry
    (
        byte idSucursal,
        FilterGrd filter
    ) : IRequest<DataGrd<ListarCobroDto>>;
}
