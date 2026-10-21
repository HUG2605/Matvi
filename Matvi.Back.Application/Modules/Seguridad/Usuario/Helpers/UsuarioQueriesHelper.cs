using Matvi.Back.Application.Modules.Seguridad.Usuario.Dtos;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Helpers
{
    public static class UsuarioQueriesHelper
    {
        public static IQueryable<RelacionarSucursalDto> GetSucursalesRelacionadas(
            MatviQryDbCtx db,
            int idCtUsuario)
        {
            return from suc in db.Set<CtSucursalEtt>()
                   join rel in db.Set<RlUsuarioSucursalEtt>()
                       on suc.Id equals rel.IdCtSucursal into relJoin
                   from rel in relJoin.Where(r => r.IdCtUsuario == idCtUsuario).DefaultIfEmpty()
                   select new RelacionarSucursalDto
                   {
                       Id = suc.Id,
                       Nombre = suc.Nombre,
                       Direccion = suc.Direccion,
                       EstaRelacionada = rel != null
                   };
        }
    }

}
