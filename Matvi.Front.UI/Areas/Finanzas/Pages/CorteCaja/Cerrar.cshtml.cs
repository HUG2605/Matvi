using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Commands;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.CorteCaja
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para cerrar.
    /// </summary>
[AuthorizeMatvi(Funcion.Finanzas_Cortes_de_caja, Accion.Crear)]
    public class CerrarModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
    ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa corte.
        /// </summary>
[BindProperty]
        public required CerrarCorteCajaDto Corte { get; set; }

                /// <inheritdoc/>
public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            Corte = new CerrarCorteCajaDto();

            if (_usuarioActualService.EstaSucursalEnSesionYRelacionada == true)
            {
                var cajaActiva = await _mediator.Send(new ObtenerCajaActivaQry(
                    _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value,
                    _usuarioActualService.UsuarioSesion!.Id), cancellationToken);

                Corte.Id = cajaActiva?.Id ?? 0;
            }
        }

                /// <summary>
        /// Ejecuta la operación de on post cerrar async.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnPostCerrarAsync(CancellationToken cancellationToken)
        {
            Corte.CtSucu_Id = _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value;
            Corte.CtUsua_Id = _usuarioActualService.UsuarioSesion!.Id;

            var res = await _mediator.Send(new CerrarCorteCajaCmd(Corte), cancellationToken);
            return new JsonResult(res);
        }
    }
}