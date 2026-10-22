using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Commands;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.CorteCaja
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para abrir.
    /// </summary>
[AuthorizeMatvi(Funcion.Finanzas_Cortes_de_caja, Accion.Crear)]
    public class AbrirModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
    ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa corte.
        /// </summary>
[BindProperty]
        public required AbrirCorteCajaDto Corte { get; set; }

                /// <summary>
        /// Texto descriptivo o detalle de nombre sucursal.
        /// </summary>
public string NombreSucursal { get; set; } = string.Empty;
                /// <summary>
        /// Texto descriptivo o detalle de nombre usuario.
        /// </summary>
public string NombreUsuario { get; set; } = string.Empty;

                /// <inheritdoc/>
public void OnGet()
        {
            var sucursal = _usuarioActualService.UsuarioSesion?.SucursalSesion;
            NombreSucursal = sucursal != null ? $"{sucursal.Clave} - {sucursal.Nombre}" : "SUCURSAL EN SESIÓN";
            NombreUsuario = _usuarioActualService.UsuarioSesion?.Nombre ?? "USUARIO ACTUAL";
        }

                /// <summary>
        /// Ejecuta la operación de on post abrir async.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnPostAbrirAsync(CancellationToken cancellationToken)
        {
            Corte.CtSucu_Id = _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value;
            Corte.CtUsua_Id = _usuarioActualService.UsuarioSesion!.Id;

            var res = await _mediator.Send(new AbrirCorteCajaCmd(Corte), cancellationToken);
            return new JsonResult(res);
        }
    }
}