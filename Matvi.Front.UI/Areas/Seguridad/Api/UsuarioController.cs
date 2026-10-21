using Matvi.Back.Application.Modules.Seguridad.Usuario.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Seguridad.Api
{
    [Area("Seguridad")]
    [Route("/Seguridad/api/usuario")]
    [ApiController]
    public class UsuarioController(
        IMediator _mediator
        ) : Controller
    {
        [HttpGet("repartidor-select")]
        public async Task<IActionResult> RepartidorSelect(
            string coincidencia,
            CancellationToken cancellationToken)
        {
            var repartidores = await _mediator.Send(
                new ListarRepartidorSelectQry(coincidencia ?? string.Empty),
                cancellationToken);

            return Ok(repartidores);
        }
    }
}