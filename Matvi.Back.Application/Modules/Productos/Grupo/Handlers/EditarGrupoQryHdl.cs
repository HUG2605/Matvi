using Matvi.Back.Application.Modules.Productos.Grupo.Dtos;
using Matvi.Back.Application.Modules.Productos.Grupo.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Handlers
{
    class EditarGrupoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<EditarGrupoQry, EditarGrupoDto>
    {
        public async Task<EditarGrupoDto> Handle(
            EditarGrupoQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtGrupoEtt>()
                .Where(cg => cg.Id == request.id)
                .Select(cg => new EditarGrupoDto
                {
                    Id = cg.Id,
                    NombreFld = cg.Nombre,
                    DescripcionFld = cg.Descripcion,
                    CtSecc_IdFld = cg.IdCtSeccion
                });

            var res = await query.SingleAsync(cancellationToken);
            return res;
        }
    }
}
