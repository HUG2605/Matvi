using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Finanzas.Pago.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.Pago.Queries
{
    public record ListarPagoQry
    (
        byte idSucucursal,
        FilterGrd filter
    ) : IRequest<DataGrd<ListarPagoDto>>;
    
}
