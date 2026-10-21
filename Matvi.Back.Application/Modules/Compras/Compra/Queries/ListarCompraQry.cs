using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Compras.Compra.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Compras.Compra.Queries
{
    public record ListarCompraQry
    (
        byte idSucursal,
        FilterGrd filter
    ): IRequest<DataGrd<ListarCompraDto>>;
         

}