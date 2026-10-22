using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Common.Dtos
{
    /// <summary>
    /// Contrato que define los campos descriptivos básicos de un producto.
    /// </summary>
    public interface IProductoBaseDto
    {
        /// <summary>
        /// Nombre comercial del producto.
        /// </summary>
        string CtProd_Nombre { get; set; }

        /// <summary>
        /// Especificación técnica o características del producto.
        /// </summary>
        string? CtProd_EspecificacionTecnica { get; set; }

        /// <summary>
        /// Marca comercial del producto.
        /// </summary>
        string? CtProd_Marca { get; set; }

        /// <summary>
        /// Nombre completo formateado que combina nombre, especificación y marca.
        /// </summary>
        string CtProd_NombreCompleto { get; }
    }
}
