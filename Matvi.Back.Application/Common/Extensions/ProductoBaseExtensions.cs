using Matvi.Back.Application.Common.Dtos;

namespace Matvi.Back.Application.Common.Extensions
{
        /// <summary>
    /// Representa la clase para producto base extensions.
    /// </summary>
public static class ProductoBaseExtensions
    {
                /// <summary>
        /// Recupera la información correspondiente a get nombre completo.
        /// </summary>
        /// <param name="dto">Parámetro dto.</param>
        /// <returns>Objeto con la información resultante de get nombre completo.</returns>
public static string GetNombreCompleto(this IProductoBaseDto dto)
        {
            return 
                dto.CtProd_Nombre
                + (string.IsNullOrEmpty(dto.CtProd_EspecificacionTecnica)
                    ? ""
                    : $" ({dto.CtProd_EspecificacionTecnica})")
                + (string.IsNullOrEmpty(dto.CtProd_Marca)
                    ? ""
                    : $" [{dto.CtProd_Marca}]");
        }
    }
}
