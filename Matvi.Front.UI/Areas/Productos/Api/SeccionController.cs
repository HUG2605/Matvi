using Matvi.Back.Application.Modules.Productos.Seccion.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Productos.Api
{
        /// <summary>
    /// Controlador de API para endpoints relacionados con seccion.
    /// </summary>
[Area("Productos")]
    [Route("/Productos/api/seccion")]
    [ApiController]
    public class SeccionController(
        IMediator _mediator
        ) : Controller
    {
                /// <summary>
        /// Ejecuta la operación de select.
        /// </summary>
        /// <param name="descripcionGrupo">Parámetro descripcion grupo.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
[HttpGet("select")]
        public async Task<IActionResult> Select(
            string descripcionGrupo, 
            CancellationToken cancellationToken)
        {
            var grupos = await _mediator.Send(
                new ListarSeccionSelectDescGrupQry(descripcionGrupo), 
                cancellationToken);

            return Ok(grupos);
        }
    }
}
