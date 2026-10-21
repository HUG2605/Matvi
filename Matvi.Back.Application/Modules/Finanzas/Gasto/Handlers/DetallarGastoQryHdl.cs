using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Dtos;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.Gasto.Handlers
{
    public class DetallarGastoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<DetallarGastoQry, DetallarGastoDto>
    {
        public async Task<DetallarGastoDto> Handle(DetallarGastoQry request, CancellationToken cancellationToken)
        {
            using var context = await _factory.CreateDbContextAsync(cancellationToken);
            
            var query = context.Set<OpGastoEtt>()
                .Where(g => g.Id == request.id)
                .Select(g => new DetallarGastoDto
                {
                    Id = g.Id,
                    CtSucu_Clave = g.Nav_IdCtSucursal.Clave,
                    Numero = g.Numero,
                    FechaPago = g.FechaPago,
                    CtTipoGast_Nombre = g.Nav_IdCtTipoGasto.Nombre,
                    Proveedor = g.Proveedor,
                    Concepto = g.Concepto,
                    Importe = g.Importe,
                    CtMedioPago_Nombre = g.Nav_IdCtMedioPago.Nombre,
                    CtUsua_Nombre = g.Nav_IdCtUsuario.Nombre,
                    Notas = g.Notas
                });
            
            var gasto = await query.SingleAsync(cancellationToken);
            return gasto;
        }
    }
}
