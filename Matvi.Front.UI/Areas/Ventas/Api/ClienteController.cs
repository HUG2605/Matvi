using Matvi.Back.Application.Modules.Ventas.Cliente.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Ventas.Api
{
    [Area("Ventas")]
    [Route("/Ventas/api/cliente")]
    [ApiController]
    public class ClienteController(
          IMediator _mediator
        ) : Controller
    {
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
