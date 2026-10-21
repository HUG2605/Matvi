using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Inventarios.Movimiento.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Inventarios.Movimiento.Queries
{
    public record ListarMovimientoQry(
        byte idSucursal,
        FilterGrd filter
        ) : IRequest<DataGrd<ListarMovimientoDto>>;
}
