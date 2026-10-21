using Matvi.Back.Application.Common.Results;
using Matvi.Front.UI.Security.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Pages
{
    [Authorize]
    public class IndexModel(
        IUsuarioActualService _usuarioActualService
        ) : BaseModel
    {
        public void OnGet()
        {
            // SOLO PARA GENERAR UNA VEZ
            // LAS CLAVES VAPID PARA LAS NOTIFICACIONES PUSH
            //var keys = VapidHelper.GenerateVapidKeys();
            //Console.WriteLine($PublicKey:  $keys.PublicKey");
            //Console.WriteLine($PrivateKey: $keys.PrivateKey");
        }

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