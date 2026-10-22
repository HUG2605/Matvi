using Matvi.Back.Application.Modules.Productos.Producto.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Productos.Api
{
        /// <summary>
    /// Controlador de API para endpoints relacionados con producto.
    /// </summary>
[Area("Productos")]
    [Route("/Productos/Api/Producto")]
    [ApiController]
    public class ProductoController(
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
            var productos = await _mediator.Send(
                new ListarProductoSelectCoincidenciaQry(coincidencia), 
                cancellationToken);

            return Ok(productos);
        }
    }
}
