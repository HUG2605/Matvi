using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Finanzas.Cobro.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.Cobro.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar cobro.
    /// </summary>
public record ListarCobroQry
    (
        byte idSucursal,
        FilterGrd filter
    ) : IRequest<DataGrd<ListarCobroDto>>;
}
