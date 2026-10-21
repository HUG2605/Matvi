using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Queries
{
    public record ListarCorteCajaQry(
        byte idSucursal,
        byte idRol,
        short idUsuario,
        FilterGrd filter
    ) : IRequest<DataGrd<ListarCorteCajaDto>>;
}