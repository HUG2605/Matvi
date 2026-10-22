using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Seguridad.Rol.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para rol funcion.
    /// </summary>
public class RolFuncionDto : IIdDto<int>
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
public byte FuncionId { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de funcion nombre.
        /// </summary>
public string? FuncionNombre { get; set; }
                /// <summary>
        /// Identificador único de modulo id.
        /// </summary>
public byte ModuloId { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de modulo nombre.
        /// </summary>
public string? ModuloNombre { get; set; }
                /// <summary>
        /// Propiedad que representa consultar.
        /// </summary>
public bool Consultar { get; set; }
                /// <summary>
        /// Propiedad que representa ver sensibles.
        /// </summary>
public bool VerSensibles { get; set; }
                /// <summary>
        /// Colección de elementos asociados a acciones.
        /// </summary>
public List<RolAccionAccesoPermisoDto>? Acciones { get; set; }
    }
}
