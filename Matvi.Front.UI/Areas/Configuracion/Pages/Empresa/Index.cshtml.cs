using Matvi.Back.Application.Modules.Configuracion.Empresa.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Configuracion.Pages.Empresa
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para index.
    /// </summary>
[AuthorizeMatvi(Funcion.Configuracion_Empresa)]
    public class IndexModel(
        IMediator _mediator
        ) : BaseModel
    {
                /// <inheritdoc/>
public void OnGet()
        {
        }

                /// <summary>
        /// Ejecuta la operación de on get empresas json async.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnGetEmpresasJsonAsync(
            CancellationToken cancellationToken)
        {
            var empresasJson = await _mediator.Send(
                new ListarEmpresaGridQry(), 
                cancellationToken);
            return new JsonResult(empresasJson);
        }
    }
}
