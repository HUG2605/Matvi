using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Handlers
{
    class ListarRepartidorSelectQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarRepartidorSelectQry, List<BaseDto<short>>>
    {
                /// <inheritdoc/>
public async Task<List<BaseDto<short>>> Handle(
            ListarRepartidorSelectQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var coincidencia = request.coincidencia ?? string.Empty;

            var query = db.Set<CtUsuarioEtt>()
                .Where(cu => cu.Activo
                          && cu.Nav_IdCtRol!.Id == (byte)RolEnm.REPARTIDOR)
                .Where(cu => coincidencia == string.Empty
                          || EF.Functions.Like(cu.Nombre, $"%{coincidencia}%")
                          || EF.Functions.Like(cu.PrimerApellido, $"%{coincidencia}%")
                          || EF.Functions.Like(cu.SegundoApellido ?? string.Empty, $"%{coincidencia}%"))
                .OrderBy(cu => cu.Nombre)
                .Select(cu => new BaseDto<short>
                {
                    Id = cu.Id,
                    Nombre = cu.Nombre + " " + cu.PrimerApellido + " " + (cu.SegundoApellido ?? "")
                });

            return await query.Take(15).ToListAsync(cancellationToken);
        }
    }
}