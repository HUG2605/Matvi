using Matvi.Back.Application.Modules.Seguridad.Login.Dtos;

namespace Matvi.Front.UI.Security.Services
{
    public interface IUsuarioActualService
    {
        Task InicializarAsync();
        UsuarioLoginDto? UsuarioSesion { get; }
        public bool EstaActivo { get; }
        public bool EstaSucursalEnSesionYRelacionada { get; }
        bool PuedeConsultar(string funcion);
        bool TienePermiso(string funcion, string accion);
        bool PuedeVerSensibles(string funcion);
        Task AsignarSucursalASesionAsync(byte idSucursal);
        byte? SucursalId => UsuarioSesion?.SucursalSesion?.Id ?? UsuarioSesion?.Sucursales?.FirstOrDefault()?.Id;
    }
}
