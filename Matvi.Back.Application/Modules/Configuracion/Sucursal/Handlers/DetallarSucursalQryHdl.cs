using Matvi.Back.Application.Modules.Configuracion.Sucursal.Dtos;
using Matvi.Back.Application.Modules.Configuracion.Sucursal.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Configuracion.Sucursal.Handlers
{
    class DetallarSucursalQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<DetallarSucursalQry, DetallarSucursalDto>
    {
                /// <inheritdoc/>
public async Task<DetallarSucursalDto> Handle(
            DetallarSucursalQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtSucursalEtt>()
                .Where(cs => cs.Id == request.id)
                .Select(cs => new DetallarSucursalDto {
                    Id = cs.Id,
                    Nombre = cs.Nombre,
                    Clave = cs.Clave,
                    Dirección = cs.Direccion,
                    Telefono = cs.Telefono
                });
                    
            var res = await query.SingleAsync(cancellationToken);
            return res;
        }
    }
}
