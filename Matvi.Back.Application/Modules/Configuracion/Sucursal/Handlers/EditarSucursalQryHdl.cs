using Matvi.Back.Application.Modules.Configuracion.Sucursal.Dtos;
using Matvi.Back.Application.Modules.Configuracion.Sucursal.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Configuracion.Sucursal.Handlers
{
    class EditarSucursalQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<EditarSucursalQry, EditarSucursalDto>
    {
        public async Task<EditarSucursalDto> Handle(
            EditarSucursalQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtSucursalEtt>()
                .Where(cs => cs.Id == request.id)
                .Select(cs => new EditarSucursalDto {
                    Id = cs.Id,
                    NombreFld = cs.Nombre,
                    ClaveFld = cs.Clave,
                    DireccionFld = cs.Direccion,
                    TelefonoFld = cs.Telefono
                });

            var res = await query.SingleAsync(cancellationToken);
            return res;
        }
    }
}
