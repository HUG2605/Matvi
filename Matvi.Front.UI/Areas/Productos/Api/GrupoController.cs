using Matvi.Back.Application.Modules.Productos.Grupo.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Productos.Api
{
    [Area("Productos")]
    [Route("/Productos/api/grupo")]
    [ApiController]
    public class GrupoController(
        IMediator mediator
        ) : Controller
    {
        readonly IMediator _mediator = mediator;

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
