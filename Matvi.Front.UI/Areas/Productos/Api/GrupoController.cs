using Matvi.Back.Application.Modules.Productos.Grupo.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Productos.Api
{
        /// <summary>
    /// Controlador de API para endpoints relacionados con grupo.
    /// </summary>
[Area("Productos")]
    [Route("/Productos/api/grupo")]
    [ApiController]
    public class GrupoController(
        IMediator mediator
        ) : Controller
    {
        readonly IMediator _mediator = mediator;

                /// <summary>
        /// Ejecuta la operación de select.
        /// </summary>
        /// <param name="idSeccion">Identificador único del registro u objeto.</param>
        /// <param name="descripcion">Parámetro descripcion.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
[HttpGet("select")]
        public async Task<IActionResult> Select(
            byte idSeccion, 
            string descripcion, 
            CancellationToken cancellationToken)
        {
            var grupos = await _mediator.Send(
                new ListarGrupoSelectQry(idSeccion, descripcion), 
                cancellationToken);
            return Ok(grupos);
        }

                /// <summary>
        /// Ejecuta la operación de descripcion.
        /// </summary>
        /// <param name="idGrupo">Identificador único del registro u objeto.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
[HttpGet("descripcion")]
        public async Task<IActionResult> Descripcion(
            byte idGrupo, 
            CancellationToken cancellationToken)
        {
            var descripcion = await _mediator.Send(
                new ObtenerGrupoDescripcionQry(idGrupo), 
                cancellationToken);

            return Ok(descripcion);
        }
    }
}
