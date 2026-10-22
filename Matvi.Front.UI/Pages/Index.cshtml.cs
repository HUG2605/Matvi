using Matvi.Back.Application.Common.Results;
using Matvi.Front.UI.Security.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Pages
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para index.
    /// </summary>
[Authorize]
    public class IndexModel(
        IUsuarioActualService _usuarioActualService
        ) : BaseModel
    {
                /// <inheritdoc/>
public void OnGet()
        {
            // SOLO PARA GENERAR UNA VEZ
            // LAS CLAVES VAPID PARA LAS NOTIFICACIONES PUSH
            //var keys = VapidHelper.GenerateVapidKeys();
            //Console.WriteLine($PublicKey:  $keys.PublicKey");
            //Console.WriteLine($PrivateKey: $keys.PrivateKey");
        }

                /// <summary>
        /// Ejecuta la operación de on get set sesion sucursal.
        /// </summary>
        /// <param name="idSucursal">Identificador único del registro u objeto.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnGetSetSesionSucursal(byte idSucursal)
        {
            await _usuarioActualService.AsignarSucursalASesionAsync(idSucursal);
            var res = OperationResult<bool>.Success(
                true, 
                "La sucursal se estableció correctamente.");
            return new JsonResult(res);
        }

    }
}