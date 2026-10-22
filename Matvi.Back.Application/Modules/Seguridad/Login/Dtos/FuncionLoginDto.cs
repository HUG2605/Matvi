using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Seguridad.Login.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para funcion login.
    /// </summary>
public class FuncionLoginDto
    {
                /// <summary>
        /// Identificador único de id.
        /// </summary>
public byte Id { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de nombre.
        /// </summary>
public string? Nombre { get; set; }
                /// <summary>
        /// Propiedad que representa ruta.
        /// </summary>
public string? Ruta { get; set; }
                /// <summary>
        /// Propiedad que representa requiere sucursal.
        /// </summary>
public bool RequiereSucursal { get; set; }
                /// <summary>
        /// Identificador único de ct modu id.
        /// </summary>
public byte? CtModu_Id { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de ct modu nombre.
        /// </summary>
public string? CtModu_Nombre { get; set; }
                /// <summary>
        /// Propiedad que representa ct modu area.
        /// </summary>
public string? CtModu_Area { get; set; }
                /// <summary>
        /// Propiedad que representa ct modu icon name.
        /// </summary>
public string? CtModu_IconName { get; set; }
                /// <summary>
        /// Propiedad que representa ver sensibles.
        /// </summary>
public bool VerSensibles { get; set; }
    }
}
