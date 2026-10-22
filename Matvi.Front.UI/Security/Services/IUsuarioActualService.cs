using Matvi.Back.Application.Modules.Seguridad.Login.Dtos;

namespace Matvi.Front.UI.Security.Services
{
    /// <summary>
    /// Servicio para consultar la identidad, permisos y sucursal del usuario actualmente autenticado en la sesión.
    /// </summary>
    public interface IUsuarioActualService
    {
        /// <summary>
        /// Inicializa el estado del usuario actual a partir de la cookie de sesión o base de datos.
        /// </summary>
        /// <returns>Tarea que representa la inicialización asíncrona.</returns>
        Task InicializarAsync();

        /// <summary>
        /// Datos completos del usuario autenticado en la sesión actual.
        /// </summary>
        UsuarioLoginDto? UsuarioSesion { get; }

        /// <summary>
        /// Indica si el usuario actual se encuentra activo en el sistema.
        /// </summary>
        public bool EstaActivo { get; }

        /// <summary>
        /// Indica si la sucursal activa en sesión está correctamente asignada y relacionada al usuario.
        /// </summary>
        public bool EstaSucursalEnSesionYRelacionada { get; }

        /// <summary>
        /// Determina si el usuario tiene permiso para consultar la función o módulo especificado.
        /// </summary>
        /// <param name="funcion">Nombre de la función o módulo a consultar.</param>
        /// <returns><c>true</c> si tiene permiso de consulta; en caso contrario, <c>false</c>.</returns>
        bool PuedeConsultar(string funcion);

        /// <summary>
        /// Determina si el usuario tiene un permiso específico (acción sobre una función).
        /// </summary>
        /// <param name="funcion">Nombre de la función o módulo.</param>
        /// <param name="accion">Acción requerida (Crear, Editar, Eliminar, etc.).</param>
        /// <returns><c>true</c> si tiene el permiso concedido; en caso contrario, <c>false</c>.</returns>
        bool TienePermiso(string funcion, string accion);

        /// <summary>
        /// Determina si el usuario tiene autorización para ver datos confidenciales o sensibles en una función.
        /// </summary>
        /// <param name="funcion">Nombre de la función a evaluar.</param>
        /// <returns><c>true</c> si puede visualizar datos sensibles; en caso contrario, <c>false</c>.</returns>
        bool PuedeVerSensibles(string funcion);

        /// <summary>
        /// Establece y persiste la sucursal activa seleccionada en la sesión del usuario.
        /// </summary>
        /// <param name="idSucursal">Identificador de la sucursal elegida.</param>
        /// <returns>Tarea que representa la operación asíncrona.</returns>
        Task AsignarSucursalASesionAsync(byte idSucursal);

        /// <summary>
        /// Identificador de la sucursal en sesión, o de la primera sucursal asociada al usuario si no hay una seleccionada.
        /// </summary>
        byte? SucursalId => UsuarioSesion?.SucursalSesion?.Id ?? UsuarioSesion?.Sucursales?.FirstOrDefault()?.Id;
    }
}
