using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.TipoGasto.Handlers
{
    class ListarTipoGastoSelectQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarTipoGastoSelectQry, List<BaseDto<byte>>>
    {
        public async Task<List<BaseDto<byte>>> Handle(
            ListarTipoGastoSelectQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtTipoGastoEtt>()
               .Select(mg => new BaseDto<byte>
               {
                   Id = mg.Id,
                   Nombre = mg.Nombre
               })
               .OrderBy(mg => mg.Nombre);

            var res = await query.ToListAsync(cancellationToken);
            return res;
        }
    }
}
