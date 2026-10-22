using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Dtos;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.TipoGasto.Handlers
{
    class EditarTipoGastoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<EditarTipoGastoQry, EditarTipoGastoDto>
    {
                /// <inheritdoc/>
public async Task<EditarTipoGastoDto> Handle(
            EditarTipoGastoQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtTipoGastoEtt>()
                .Where(ctg => ctg.Id == request.id)
                .Select(ctg => new EditarTipoGastoDto {
                    Id = ctg.Id,
                    NombreFld = ctg.Nombre
                });

            var tipoGasto = await query.SingleAsync(cancellationToken);
            return tipoGasto;
        }
    }
}
