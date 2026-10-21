using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Compras.Compra.Queries;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Commands;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Dtos;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.Gasto
{
    [AuthorizeMatvi(Funcion.Finanzas_Gastos)]
    public class DetailsModel(
        IUsuarioActualService _usuarioActualService,
        IMediator _mediator
        ) : BaseModel
    {
        public DetallarGastoDto? Gasto { get; set; }
        public async Task<IActionResult> OnGet(
            string id,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToPage("Index");

            var idCompra = ToInt(id);
            Gasto = await _mediator.Send(
                new DetallarGastoQry(idCompra),
                cancellationToken);

            if (Gasto == null)
                return NotFound();

            return Page();

        }

        public async Task<JsonResult> OnGetDelete(
            string id,
            CancellationToken cancellationToken)
        {
            if (!_usuarioActualService.TienePermiso(
                Funcion.Finanzas_Gastos,
                Accion.Eliminar))
            {
                return new JsonResult(OperationResult<bool>.Error("Acceso denegado"));

            }

            var idCompra = ToInt(id);
            var res = await _mediator.Send(
                new EliminarGastoCmd(idCompra),
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
