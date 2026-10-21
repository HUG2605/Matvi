using Matvi.Back.Application.Modules.Finanzas.Gasto.Dtos;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.Gasto.Handlers
{
    public class EditarGastoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<EditarGastoQry, EditarGastoDto>
    {
        public async Task<EditarGastoDto> Handle(
            EditarGastoQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<OpGastoEtt>()
                .Where(i => i.Id == request.id)
                .Select(i => new EditarGastoDto
                {
                    Id = i.Id,
                    CtSucu_Id = i.IdCtSucursal,
                    CtUsua_Id = i.IdCtUsuario,
                    CtTipoGast_Id = i.IdCtTipoGasto,
                    ProveedorFld = i.Proveedor,
                    CtMediPago_IdFld = i.IdCtMedioPago,
                    ConceptoFld = i.Concepto,
                    NotasGastoFld = i.Notas,
                    ImporteFld = i.Importe,

                });

            var gasto = await query.SingleAsync(cancellationToken);
            return gasto;
        }
    }
}