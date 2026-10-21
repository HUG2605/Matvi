using Matvi.Back.Application.Modules.Productos.Producto.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Productos.Api
{
    [Area("Productos")]
    [Route("/Productos/Api/Producto")]
    [ApiController]
    public class ProductoController(
        IMediator _mediator
        ) : Controller
    {
        [HttpGet("select")]
        public async Task<IActionResult> Select(
            string coincidencia, 
            CancellationToken cancellationToken)
        {
            var productos = await _mediator.Send(
                new ListarProductoSelectCoincidenciaQry(coincidencia), 
                cancellationToken);

            return Ok(productos);
        }
    }
}
