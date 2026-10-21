using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Commands;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.CorteCaja
{
    [AuthorizeMatvi(Funcion.Finanzas_Cortes_de_caja, Accion.Crear)]
    public class AbrirModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
    ) : BaseModel
    {
        [BindProperty]
        public required AbrirCorteCajaDto Corte { get; set; }

        public string NombreSucursal { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;

        public void OnGet()
        {
            var sucursal = _usuarioActualService.UsuarioSesion?.SucursalSesion;
            NombreSucursal = sucursal != null ? $"{sucursal.Clave} - {sucursal.Nombre}" : "SUCURSAL EN SESIÓN";
            NombreUsuario = _usuarioActualService.UsuarioSesion?.Nombre ?? "USUARIO ACTUAL";
        }

        public async Task<JsonResult> OnPostAbrirAsync(CancellationToken cancellationToken)
        {
            Corte.CtSucu_Id = _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value;
            Corte.CtUsua_Id = _usuarioActualService.UsuarioSesion!.Id;

            var res = await _mediator.Send(new AbrirCorteCajaCmd(Corte), cancellationToken);
            return new JsonResult(res);
        }
    }
}