using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Matvi.Back.Application.Modules.Compras.Compra.Queries
{
    public record ObtenerSaldoVencidoQry(
        byte idsucursal,
        int idproveedor
        ): IRequest<decimal>;

}
