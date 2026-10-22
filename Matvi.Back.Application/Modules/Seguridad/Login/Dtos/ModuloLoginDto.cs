using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Seguridad.Login.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para modulo login.
    /// </summary>
public class ModuloLoginDto
    {
                /// <summary>
        /// Identificador único de id.
        /// </summary>
public int Id { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de nombre.
        /// </summary>
public string Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa area.
        /// </summary>
public string Area { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa icon name.
        /// </summary>
public string IconName { get; set; } = string.Empty;
    }

}
