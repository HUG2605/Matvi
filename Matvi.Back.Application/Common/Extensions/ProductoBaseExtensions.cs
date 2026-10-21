using Matvi.Back.Application.Common.Dtos;

namespace Matvi.Back.Application.Common.Extensions
{
    public static class ProductoBaseExtensions
    {
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
