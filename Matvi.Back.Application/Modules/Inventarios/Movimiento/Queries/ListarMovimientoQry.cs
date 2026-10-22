using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Inventarios.Movimiento.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Inventarios.Movimiento.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar movimiento.
    /// </summary>
public record ListarMovimientoQry(
        byte idSucursal,
        FilterGrd filter
        ) : IRequest<DataGrd<ListarMovimientoDto>>;
}
