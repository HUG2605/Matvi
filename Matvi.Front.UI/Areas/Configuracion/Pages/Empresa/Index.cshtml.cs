using Matvi.Back.Application.Modules.Configuracion.Empresa.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Configuracion.Pages.Empresa
{
    [AuthorizeMatvi(Funcion.Configuracion_Empresa)]
    public class IndexModel(
        IMediator _mediator
        ) : BaseModel
    {
        public void OnGet()
        {
        }

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
