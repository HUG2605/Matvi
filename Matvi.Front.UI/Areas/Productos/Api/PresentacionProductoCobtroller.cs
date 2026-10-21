using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Productos.Api
{
    [Area("Productos")]
    [Route("/Productos/Api/PresentacionProducto")]
    [ApiController]
    public class PresentacionProductoCobtroller(
        IMediator _mediator
        ) : Controller
    {
        [HttpGet("select")]
        public async Task<IActionResult> Select(
            int idProducto, 
            bool paraCompra, 
            bool paraVenta, 
            CancellationToken cancellationToken)
        {
            var presentacionProductoLst = await _mediator.Send(
                new ListarPresentacionProductoSelectQry(
                    idProducto,
                    paraCompra,
                    paraVenta), 
                cancellationToken);

            return Ok(presentacionProductoLst);
        }
    }
}
