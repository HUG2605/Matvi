using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Modules.Compras.Compra.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Compras.Compra.Handlers
{
    class ObtenerSaldoVencidoProveedorQryHdl(
        IDbContextFactory<MatviQryDbCtx> factory
        ) : IRequestHandler<ObtenerSaldoVencidoQry, decimal>
    {
        public async Task<decimal> Handle(
            ObtenerSaldoVencidoQry request,
            CancellationToken cancellationToken)
        {
            using var db = await factory.CreateDbContextAsync(cancellationToken);
            var saldo = await db.Set<OpCompraEtt>()
                .Where(c =>
                    c.IdCtSucursal == request.idsucursal &&
                    c.IdCtProveedor == request.idproveedor &&
                    c.Total > c.TotalPagado)
                .SumAsync(c => (decimal?)(c.Total - c.TotalPagado), cancellationToken) ?? 0;

            return saldo;
        }
    }
}
