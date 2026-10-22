using Matvi.Back.Application.Modules.Ventas.Cliente.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Ventas.Api
{
        /// <summary>
    /// Controlador de API para endpoints relacionados con cliente.
    /// </summary>
[Area("Ventas")]
    [Route("/Ventas/api/cliente")]
    [ApiController]
    public class ClienteController(
          IMediator _mediator
        ) : Controller
    {
                /// <summary>
        /// Ejecuta la operación de select.
        /// </summary>
        /// <param name="coincidencia">Parámetro coincidencia.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
[HttpGet("select")]
        public async Task<IActionResult> Select(
            string coincidencia, 
            CancellationToken cancellationToken)
        {
            var clientes = await _mediator.Send(
                new ListarClienteSelectCoincidenciaQry(coincidencia),
                cancellationToken);

            return Ok(clientes);
        }
    }
}
