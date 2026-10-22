using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Dtos;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Queries;
using Matvi.Back.Infrastructure.Common.Extensions;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Handlers
{
    class ListarUsuariosQryHandler(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarUsuariosQry, DataGrd<ListarUsuarioDto>>
    {
                /// <inheritdoc/>
public async Task<DataGrd<ListarUsuarioDto>> Handle(
            ListarUsuariosQry request, 
            CancellationToken cancellationToken)
        {
            #region Query base

            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtUsuarioEtt>()
                .Where(cu => cu.Nav_IdCtRol!.Id != (byte)RolEnm.SYSTEM)
                .Select(cu => new ListarUsuarioDto
                {
                    Id = cu.Id,
                    CtRol_Nombre = cu.Nav_IdCtRol!.Nombre,
                    NombreCompleto = cu.Nombre + " " + cu.PrimerApellido + " " + cu.SegundoApellido,
                    Usuario = cu.Usuario,
                    Telefono = cu.Telefono,
                    Activo = cu.Activo
                });

            #endregion

            #region Filtro por texto

            if (!string.IsNullOrWhiteSpace(request.filter.Search))
            {
                var searchLower = request.filter.Search.ToLower();
                query = query.Where(x =>
                    EF.Functions.Like(x.NombreCompleto!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtRol_Nombre!, $"%{searchLower}%") ||
                    (x.Telefono != null && EF.Functions.Like(x.Telefono!, $"%{searchLower}%"))
                );
            }

            #endregion

            #region Ordenamiento

            if (!string.IsNullOrWhiteSpace(request.filter.SortBy))
            {
                var sortBy = request.filter.SortBy.ToLower();
                var descending = request.filter.SortDesc;

                query = query.ApplyOrdering(sortBy.ToLowerInvariant(), descending);

                query = sortBy switch
                {
                    "nombrecompleto" => descending ? query.OrderByDescending(x => x.NombreCompleto) : query.OrderBy(x => x.NombreCompleto),
                    "telefonomovil" => descending ? query.OrderByDescending(x => x.Telefono) : query.OrderBy(x => x.Telefono),
                    "rolnombre" => descending ? query.OrderByDescending(x => x.CtRol_Nombre) : query.OrderBy(x => x.CtRol_Nombre),
                    "usuario" => descending ? query.OrderByDescending(x => x.Usuario) : query.OrderBy(x => x.Usuario),
                    "fechaultimoacceso" => descending ? query.OrderByDescending(x => x.FechaUltimoAcceso) : query.OrderBy(x => x.FechaUltimoAcceso),
                    "activo" => descending ? query.OrderByDescending(x => x.Activo) : query.OrderBy(x => x.Activo),
                    _ => query
                };
            }

            #endregion

            #region Total antes de paginar

            var totalItems = await query.CountAsync(cancellationToken);

            #endregion

            #region Paginación

            var items = await query
                .Skip((request.filter.Page - 1) * request.filter.PageSize)
                .Take(request.filter.PageSize)
                .ToListAsync(cancellationToken);

            #endregion

            #region Resultado

            return new DataGrd<ListarUsuarioDto>
            {
                TotalItems = totalItems,
                Items = items
            };

            #endregion 
        }
    }
}
