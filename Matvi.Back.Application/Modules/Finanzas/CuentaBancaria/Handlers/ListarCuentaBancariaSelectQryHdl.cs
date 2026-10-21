using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Modules.Finanzas.CuentaBancaria.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.CuentaBancaria.Handlers
{
    class ListarCuentaBancariaSelectQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
    ) : IRequestHandler<ListarCuentaBancariaSelectQry, List<BaseDto<byte>>>
    {
        public async Task<List<BaseDto<byte>>> Handle(
            ListarCuentaBancariaSelectQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            return await db.Set<CtCuentaBancariaEtt>()
                .AsNoTracking()
                .Where(c => c.Activa)
                .OrderBy(c => c.Banco)
                .ThenBy(c => c.Nombre)
                .Select(c => new BaseDto<byte>
                {
                    Id = c.Id,
                    Nombre = c.Banco + " - " + c.Nombre + " (" + c.NumeroCuenta + ")"
                })
                .ToListAsync(cancellationToken);
        }
    }
}
