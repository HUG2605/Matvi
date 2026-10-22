using Matvi.Back.Application.Modules.Compras.Proveedor.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Compras.Api
{
        /// <summary>
    /// Controlador de API para endpoints relacionados con proveedor.
    /// </summary>
[Area("Compras")]
    [Route("/Compras/api/proveedor")]
    [ApiController]
    public class ProveedorController(
        IMediator mediator
    ) : Controller
    {
        readonly IMediator _mediator = mediator;

                /// <summary>
        /// Ejecuta la operación de select.
        /// </summary>
        /// <param name="coincidencia">Parámetro coincidencia.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
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