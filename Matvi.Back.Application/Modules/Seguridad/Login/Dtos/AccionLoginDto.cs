using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Seguridad.Login.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para accion login.
    /// </summary>
public class AccionLoginDto
    {
                /// <summary>
        /// Propiedad que representa funcion.
        /// </summary>
public string Funcion { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa accion.
        /// </summary>
public string Accion { get; set; } = string.Empty;
    }

}
