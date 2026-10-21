using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Commands;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Dtos;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.TipoGasto
{
    [AuthorizeMatvi(Funcion.Finanzas_Tipos_de_Gastos, Accion.Crear)]
    public class CreateModel(
        IMediator _mediator
        ) : BaseModel
    {
        [BindProperty]
        public required CrearTipoGastoDto TipoGasto { get; set; }

        public void OnGet()
        {
        }

        public async Task<JsonResult> OnPostCreateAsync(CancellationToken cancellationToken)
        {
            var res = await _mediator.Send(
                new CrearTipoGastoCmd(TipoGasto), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
