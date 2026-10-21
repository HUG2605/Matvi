using Matvi.Back.Application.Modules.Configuracion.Empresa.Dtos;
using Matvi.Back.Application.Modules.Configuracion.Empresa.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Configuracion.Empresa.Handlers
{
    class EditarEmpresaQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<EditarEmpresaQry, EditarEmpresaDto>
    {
        public async Task<EditarEmpresaDto> Handle(
            EditarEmpresaQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtEmpresaEtt>()
                .Where(ce => ce.Id == request.id)
                .Select(ce => new EditarEmpresaDto
                {
                    Id = ce.Id,
                    NombreFld = ce.Nombre,
                    LogotipoFld = ce.Logotipo,
                    TelefonoFld = ce.Telefono
                });

            var res = await query.SingleAsync(cancellationToken);
            return res;
        }
    }
}
