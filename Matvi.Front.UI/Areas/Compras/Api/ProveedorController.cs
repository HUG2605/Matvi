using Matvi.Back.Application.Modules.Compras.Proveedor.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Compras.Api
{
    [Area("Compras")]
    [Route("/Compras/api/proveedor")]
    [ApiController]
    public class ProveedorController(
        IMediator mediator
    ) : Controller
    {
        readonly IMediator _mediator = mediator;

        [HttpGet("select")]
        public async Task<IActionResult> Select(string coincidencia, CancellationToken cancellationToken)
        {
            var proveedores = await _mediator.Send(
                new ListarProveedorSelectCoincidenciaQry(coincidencia),
                cancellationToken);

            return Ok(proveedores);
        }
    }
}