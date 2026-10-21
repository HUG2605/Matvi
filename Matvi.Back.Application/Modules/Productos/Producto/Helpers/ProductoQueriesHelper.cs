using Matvi.Back.Application.Modules.Productos.Producto.Dtos;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Application.Modules.Productos.Producto.Helpers
{
    public static class ProductoQueriesHelper
    {
        public static IQueryable<RelacionarProveedorDto> GetProveedoresRelacionados(
         MatviQryDbCtx db,
         int idCtProducto)
        {
            return from pro in db.Set<CtProveedorEtt>()
                   join rel in db.Set<RlProductoProveedorEtt>()
                       on pro.Id equals rel.IdCtProveedor into relJoin
                   from rel in relJoin.Where(r => r.IdCtProducto == idCtProducto).DefaultIfEmpty()
                   orderby pro.Nombre ascending
                   select new RelacionarProveedorDto
                   {
                       CtProv_Id = pro.Id,
                       Nombre = pro.Nombre,
                       EstaRelacionado = rel != null
                   };
        }

        public static string GenerarCodigoInterno(int idProducto)
        {
            return $"CBI-{idProducto.ToString("D6")}";
        }
    }
}
