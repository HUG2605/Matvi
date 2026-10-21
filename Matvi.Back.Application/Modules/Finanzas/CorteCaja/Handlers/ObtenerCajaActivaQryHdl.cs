using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Handlers
{
    public class ObtenerCajaActivaQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
    ) : IRequestHandler<ObtenerCajaActivaQry, CajaActivaDto?>
    {
        public async Task<CajaActivaDto?> Handle(
            ObtenerCajaActivaQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            return await db.Set<OpCorteCajaEtt>()
                .Where(c =>
                    c.IdCtSucursal == request.idSucursal &&
                    c.IdCtUsuario == request.idUsuario &&
                    c.FechaCierre == null)
                .Select(c => new CajaActivaDto
                {
                    Id = c.Id,
                    CtSucu_Clave = c.Nav_IdCtSucursal.Clave,
                    Numero = c.Numero,
                    FechaApertura = c.FechaApertura,
                    FondoInicial = c.FondoInicial
                })
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}