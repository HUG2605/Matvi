using Matvi.Back.Application.Modules.Configuracion.Empresa.Dtos;
using Matvi.Back.Application.Modules.Configuracion.Empresa.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Configuracion.Empresa.Handlers
{
    class DetallarEmpresaQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<DetallarEmpresaQry, DetallarEmpresaDto>
    {
        public async Task<DetallarEmpresaDto> Handle(
            DetallarEmpresaQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtEmpresaEtt>()
                .Where(ce => ce.Id == request.id)
                .Select(ce => new DetallarEmpresaDto
                {
                    Id = ce.Id,
                    Nombre = ce.Nombre,
                    Logotipo = ce.Logotipo,
                    Telefono = ce.Telefono
                });

            var res = await query.SingleAsync(cancellationToken);
            return res;
        }
    }
}
