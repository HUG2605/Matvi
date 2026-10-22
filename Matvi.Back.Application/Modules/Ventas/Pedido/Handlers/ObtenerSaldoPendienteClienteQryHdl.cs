using Matvi.Back.Application.Modules.Ventas.Pedido.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Handlers
{
    class ObtenerSaldoPendienteClienteQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
    ) : IRequestHandler<ObtenerSaldoPendienteClienteQry, decimal>
    {
                /// <inheritdoc/>
public async Task<decimal> Handle(
            ObtenerSaldoPendienteClienteQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var saldo = await db.Set<OpPedidoEtt>()
                .Where(p =>
                    p.IdCtSucursal == request.IdSucursal &&
                    p.IdCtCliente == request.IdCliente &&
                    p.Total > p.TotalPagado)
                .SumAsync(p => (decimal?)(p.Total - p.TotalPagado), cancellationToken) ?? 0;

            return saldo;
        }
    }
}