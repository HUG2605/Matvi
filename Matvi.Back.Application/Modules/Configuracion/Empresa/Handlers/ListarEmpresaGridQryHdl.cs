using Matvi.Back.Application.Modules.Configuracion.Empresa.Dtos;
using Matvi.Back.Application.Modules.Configuracion.Empresa.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Configuracion.Empresa.Handlers
{
    class ListarEmpresaGridQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarEmpresaGridQry, List<ListarEmpresaDto>>
    {
                /// <inheritdoc/>
public async Task<List<ListarEmpresaDto>> Handle(
            ListarEmpresaGridQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtEmpresaEtt>()
                .Select(ce => new ListarEmpresaDto
                {
                    Id = ce.Id,
                    Nombre = ce.Nombre,
                    Telefono = ce.Telefono
                });

            var res = await query.ToListAsync(cancellationToken);
            return res;

        }
    }
}
