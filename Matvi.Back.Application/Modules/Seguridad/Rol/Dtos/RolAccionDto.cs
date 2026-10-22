using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Seguridad.Rol.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para rol accion acceso permiso.
    /// </summary>
public class RolAccionAccesoPermisoDto : IIdDto<int>
    {
        #region Implementacion de IIdDto

                /// <summary>
        /// Identificador único de id.
        /// </summary>
public int Id { get; set; }
                /// <summary>
        /// Identificador único de safe id.
        /// </summary>
public string SafeId => Id.GetSafeId();

        #endregion

                /// <summary>
        /// Identificador único de funcion id.
        /// </summary>
public int FuncionId { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de accion nombre.
        /// </summary>
public string? AccionNombre { get; set; }
                /// <summary>
        /// Propiedad que representa tiene acceso.
        /// </summary>
public bool TieneAcceso { get; set; }
    }
}
