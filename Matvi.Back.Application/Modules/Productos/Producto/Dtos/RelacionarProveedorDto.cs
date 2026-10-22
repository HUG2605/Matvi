using Matvi.Back.Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Productos.Producto.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para relacionar proveedor.
    /// </summary>
public class RelacionarProveedorDto
    {
                /// <summary>
        /// Identificador único de ct prov id.
        /// </summary>
public int CtProv_Id { get; set; } 
                /// <summary>
        /// Texto descriptivo o detalle de nombre.
        /// </summary>
public string Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Indica si esta relacionado se encuentra activo o habilitado.
        /// </summary>
public bool EstaRelacionado { get; set; }
    }
}
