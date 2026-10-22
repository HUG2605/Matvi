namespace Matvi.Back.Application.Modules.Seguridad.Login.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para usuario login.
    /// </summary>
public class UsuarioLoginDto
    {
                /// <summary>
        /// Identificador único de id.
        /// </summary>
public short Id { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de nombre.
        /// </summary>
public string? Nombre { get; set; }
                /// <summary>
        /// Propiedad que representa primer apellido.
        /// </summary>
public string? PrimerApellido { get; set; }
                /// <summary>
        /// Propiedad que representa segundo apellido.
        /// </summary>
public string? SegundoApellido { get; set; }
                /// <summary>
        /// Identificador único de rol id.
        /// </summary>
public byte? RolId { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de rol nombre.
        /// </summary>
public string? RolNombre { get; set; }
                /// <summary>
        /// Indica si activo se encuentra activo o habilitado.
        /// </summary>
public bool Activo { get; set; }
                /// <summary>
        /// Colección de elementos asociados a funciones.
        /// </summary>
public List<FuncionLoginDto>? Funciones { get; set; }
                /// <summary>
        /// Colección de elementos asociados a acciones.
        /// </summary>
public List<AccionLoginDto>? Acciones { get; set; }
                /// <summary>
        /// Colección de elementos asociados a sucursales.
        /// </summary>
public List<SucursalLoginDto>? Sucursales { get; set; }
                /// <summary>
        /// Propiedad que representa sucursal sesion.
        /// </summary>
public SucursalLoginDto? SucursalSesion { get; set; }

    }


}
