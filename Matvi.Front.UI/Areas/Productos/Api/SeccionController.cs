using Matvi.Back.Application.Modules.Productos.Seccion.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Productos.Api
{
    [Area("Productos")]
    [Route("/Productos/api/seccion")]
    [ApiController]
    public class SeccionController(
        IMediator _mediator
        ) : Controller
    {
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
