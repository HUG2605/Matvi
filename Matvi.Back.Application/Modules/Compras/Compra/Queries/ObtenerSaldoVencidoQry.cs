using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Matvi.Back.Application.Modules.Compras.Compra.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de obtener saldo vencido.
    /// </summary>
public record ObtenerSaldoVencidoQry(
        byte idsucursal,
        int idproveedor
        ): IRequest<decimal>;

}
