using Matvi.Back.Application.Modules.Seguridad.Usuario.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Seguridad.Api
{
        /// <summary>
    /// Controlador de API para endpoints relacionados con usuario.
    /// </summary>
[Area("Seguridad")]
    [Route("/Seguridad/api/usuario")]
    [ApiController]
    public class UsuarioController(
        IMediator _mediator
        ) : Controller
    {
                /// <summary>
        /// Ejecuta la operación de repartidor select.
        /// </summary>
        /// <param name="coincidencia">Parámetro coincidencia.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
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