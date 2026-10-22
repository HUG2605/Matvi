using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Seguridad.Login.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para sucursal login.
    /// </summary>
public class SucursalLoginDto
    {
                /// <summary>
        /// Identificador único de id.
        /// </summary>
public byte? Id { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de nombre.
        /// </summary>
public string Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa clave.
        /// </summary>
public string Clave { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa telefono.
        /// </summary>
public string Telefono { get; set; } = string.Empty;
    }
}