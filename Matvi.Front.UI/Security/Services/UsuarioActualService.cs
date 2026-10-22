using Matvi.Back.Application.Modules.Seguridad.Login.Dtos;
using Matvi.Back.Application.Modules.Seguridad.Login.Queries;
using Matvi.Front.UI.Constants;
using Matvi.Front.UI.Extensions;
using MediatR;
using System.Security.Claims;

namespace Matvi.Front.UI.Security.Services
{
        /// <summary>
    /// Representa la clase para usuario actual service.
    /// </summary>
public class UsuarioActualService (
        IHttpContextAccessor _httpContextAccessor,
        IMediator _mediator
        ) : IUsuarioActualService
    {
        UsuarioLoginDto? _usuSesDto;
        ClaimsPrincipal? _user;
        bool _estaActivo;
        bool _estaSucursalEnSesionYRelacionada;

                /// <summary>
        /// Ejecuta la operación de inicializar async.
        /// </summary>
public async Task InicializarAsync()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context?.User == null)
                return;

            _user = context.User;

            var idStr = _user.FindFirst(AuthConstants.UserSessionIdKey)?.Value;
            if (!short.TryParse(idStr, out var idUsuarioSession))
                return;

            _usuSesDto = context.Session.GetObject<UsuarioLoginDto>(AuthConstants.UserSessionKey);

            if (_usuSesDto == null)
            {
                _usuSesDto = await _mediator.Send(
                    new ObtenerUsuarioSesionQry(idUsuarioSession), 
                    context.RequestAborted);
                context.Session.SetObject(AuthConstants.UserSessionKey, _usuSesDto);
            }

            _estaActivo = await _mediator.Send(
                new VerificarEsUsuarioActivoQry(idUsuarioSession), 
                context.RequestAborted);

            if (!_estaActivo)
            {
                context.Session.Remove(AuthConstants.UserSessionKey);
                return;
            }

            if (_usuSesDto != null && _usuSesDto.SucursalSesion == null && _usuSesDto.Sucursales?.Count > 0)
            {
                _usuSesDto.SucursalSesion = _usuSesDto.Sucursales.First();
                context.Session.SetObject(AuthConstants.UserSessionKey, _usuSesDto);
            }

            if (_usuSesDto?.SucursalSesion != null)
            {
                _estaSucursalEnSesionYRelacionada = await _mediator.Send(
                    new VerificarExisteRelUsuarioSucursalQry(
                        idUsuarioSession,
                        _usuSesDto.SucursalSesion.Id!.Value
                    ),
                    context.RequestAborted);
            }
        }

                /// <summary>
        /// Propiedad que representa usuario sesion.
        /// </summary>
public UsuarioLoginDto? UsuarioSesion => _usuSesDto;

                /// <summary>
        /// Indica si esta activo se encuentra activo o habilitado.
        /// </summary>
public bool EstaActivo => _estaActivo;

                /// <summary>
        /// Indica si esta sucursal en sesion y relacionada se encuentra activo o habilitado.
        /// </summary>
public bool EstaSucursalEnSesionYRelacionada => _estaSucursalEnSesionYRelacionada;

                /// <summary>
        /// Ejecuta la operación de puede consultar.
        /// </summary>
        /// <param name="funcion">Parámetro funcion.</param>
        /// <returns><c>true</c> si la operación se completó exitosamente; en caso contrario, <c>false</c>.</returns>
public bool PuedeConsultar(string funcion)
            => _user?.HasClaim("permiso", $"{funcion}:Consultar") ?? false;

                /// <summary>
        /// Ejecuta la operación de tiene permiso.
        /// </summary>
        /// <param name="funcion">Parámetro funcion.</param>
        /// <param name="accion">Parámetro accion.</param>
        /// <returns><c>true</c> si la operación se completó exitosamente; en caso contrario, <c>false</c>.</returns>
public bool TienePermiso(string funcion, string accion)
            => _user?.HasClaim("permiso", $"{funcion}:{accion}") ?? false;

                /// <summary>
        /// Ejecuta la operación de puede ver sensibles.
        /// </summary>
        /// <param name="funcion">Parámetro funcion.</param>
        /// <returns><c>true</c> si la operación se completó exitosamente; en caso contrario, <c>false</c>.</returns>
public bool PuedeVerSensibles(string funcion)
            => _user?.HasClaim($"verSensibles:{funcion}", "true") ?? false;

                /// <summary>
        /// Ejecuta la operación de asignar sucursal a sesion async.
        /// </summary>
        /// <param name="idSucursal">Identificador único del registro u objeto.</param>
public Task AsignarSucursalASesionAsync(byte idSucursal)
        {
            if (_usuSesDto is not null)
            {
                _usuSesDto!.SucursalSesion = _usuSesDto.Sucursales!
                    .Single(i => i.Id == idSucursal);

                _httpContextAccessor.HttpContext!.Session.SetObject(AuthConstants.UserSessionKey, _usuSesDto);
                _estaSucursalEnSesionYRelacionada = true;
            }

            return Task.CompletedTask;
        }
    }
}
