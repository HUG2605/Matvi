using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Productos.Api
{
        /// <summary>
    /// Representa la clase para presentacion producto cobtroller.
    /// </summary>
[Area("Productos")]
    [Route("/Productos/Api/PresentacionProducto")]
    [ApiController]
    public class PresentacionProductoCobtroller(
        IMediator _mediator
        ) : Controller
    {
                /// <summary>
        /// Ejecuta la operación de select.
        /// </summary>
        /// <param name="idProducto">Identificador único del registro u objeto.</param>
        /// <param name="paraCompra">Parámetro para compra.</param>
        /// <param name="paraVenta">Parámetro para venta.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
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
