using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Modules.Productos.Grupo.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Handlers
{
    class ListarGrupoSelectQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarGrupoSelectQry, List<BaseDto<short>>>
    {
        public async Task<List<BaseDto<short>>> Handle(
            ListarGrupoSelectQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var stopwords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "DE", "DEL", "Y", "PARA"
            };

            var palabras = request.descripcion
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(p => !stopwords.Contains(p))
                .ToList();

            var query = db.Set<CtGrupoEtt>()
                .Where(cg => cg.IdCtSeccion == request.idCatSeccion)
                .Select(cg => new BaseDto<short> {
                    Id = cg.Id,
                    Nombre = palabras.Any(p =>
                                cg.Nombre.Contains(p) ||
                                cg.Descripcion.Contains(p))
                                     ? "★ " + cg.Nombre
                                     : cg.Nombre
                });

            var res = await query.ToListAsync(cancellationToken);
            return res;
        }
    }
}
