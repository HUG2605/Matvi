using Matvi.Back.Application.Modules.Productos.Producto.Dtos;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Application.Modules.Productos.Producto.Helpers
{
        /// <summary>
    /// Representa la clase para producto queries helper.
    /// </summary>
public static class ProductoQueriesHelper
    {
                /// <summary>
        /// Recupera la información correspondiente a get proveedores relacionados.
        /// </summary>
        /// <param name="db">Parámetro db.</param>
        /// <param name="idCtProducto">Identificador único del registro u objeto.</param>
        /// <returns>Objeto con la información resultante de get proveedores relacionados.</returns>
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

                /// <summary>
        /// Ejecuta la operación de generar codigo interno.
        /// </summary>
        /// <param name="idProducto">Identificador único del registro u objeto.</param>
        /// <returns>Objeto con la información resultante de generar codigo interno.</returns>
public static string GenerarCodigoInterno(int idProducto)
        {
            return $"CBI-{idProducto.ToString("D6")}";
        }
    }
}
