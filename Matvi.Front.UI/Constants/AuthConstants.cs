namespace Matvi.Front.UI.Constants
{
    /// <summary>
    /// Constantes de configuración de esquema de autenticación y claves de sesión de usuario.
    /// </summary>
    public class AuthConstants
    {
        /// <summary>
        /// Nombre del esquema de autenticación por cookies para Matvi.
        /// </summary>
        public const string Scheme = "MatviAuthScheme";

        /// <summary>
        /// Nombre de la cookie de autenticación de sesión.
        /// </summary>
        public const string CookieName = "matvi.auth.session";

        /// <summary>
        /// Clave empleada para almacenar la información del usuario en sesión.
        /// </summary>
        public const string UserSessionKey = "UsuarioSesion";

        /// <summary>
        /// Clave empleada para almacenar el identificador del usuario en sesión.
        /// </summary>
        public const string UserSessionIdKey = "UsuarioSesionId";
    }
}
