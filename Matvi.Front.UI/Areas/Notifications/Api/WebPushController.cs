using Matvi.Back.Application.Modules.Notifications.WebPush.Commands;
using Matvi.Back.Application.Modules.Notifications.WebPush.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Notifications.Api
{
    [Authorize]
    [Route("/Notifications/api/webpush")]
    [ApiController]
    public class WebPushController(
        IMediator _mediator
        ) : Controller
    {
        [HttpPost("suscribir")]
        public async Task<IActionResult> Suscribir(
            [FromBody] WebPushSuscripcionDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GuardarWebPushSuscripcionCmd(dto),
                cancellationToken);

            return result.IsSuccessful ? Ok() : BadRequest(result.Message);
        }


        [HttpGet("test")]
        public async Task<IActionResult> Test(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new EnviarWebPushCmd(
                    Titulo: "🔔 Prueba Matvi",
                    Cuerpo: "Si ves esto, las notificaciones funcionan.",
                    Datos: new { tipo = "TEST" }),
                cancellationToken);

            return result.IsSuccessful ? Ok(result.Message) : BadRequest(result.Message);
        }

    }
}
