using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Modules.Productos.Seccion.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Handlers
{
    class ListarSeccionSelectDescGrupQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarSeccionSelectDescGrupQry, List<BaseDto<byte>>>
    {
        public async Task<List<BaseDto<byte>>> Handle(
            ListarSeccionSelectDescGrupQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var stopwords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "DE", "DEL", "Y", "PARA"
            };

            var palabras = request.descripcionGrupo
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(p => !stopwords.Contains(p))
                .ToList();

            var query = db.Set<CtSeccionEtt>()
                .Select(cs => new BaseDto<byte> {
                    Id = cs.Id,
                    Nombre = cs.Coll_CtGrupo_IdCtSeccion!
                        .Any(g => palabras.Any(p => g.Descripcion.Contains(p)))
                        ? "★ " + cs.Nombre
                        : cs.Nombre
                });
                      
            var res = await query.ToListAsync(cancellationToken);
            return res;
        }
    }
}
