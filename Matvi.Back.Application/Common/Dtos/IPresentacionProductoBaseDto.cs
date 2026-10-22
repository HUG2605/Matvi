using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Common.Dtos
{
    /// <summary>
    /// Contrato que define los campos base compartidos para presentaciones de producto.
    /// </summary>
    public interface IPresentacionProductoBaseDto
    {
        /// <summary>
        /// Identificador del producto al que pertenece la presentación.
        /// </summary>
        int CtPresProd_IdCtProducto { get; set; }

        /// <summary>
        /// Nombre descriptivo de la presentación de producto.
        /// </summary>
        string CtPresProd_Nombre { get; set; }

        /// <summary>
        /// Factor de conversión respecto a la Unidad Base de Inventario (UBI).
        /// </summary>
        decimal CtPresProd_FactorUBI { get; set; }
    }
}
