using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Dto;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.CorteCaja
{
    [AuthorizeMatvi(Funcion.Finanzas_Cortes_de_caja)]
    public class DetailsModel(
        IMediator _mediator
    ) : BaseModel
    {
        public DetallarCorteCajaDto? Corte { get; set; }

        public async Task<IActionResult> OnGet(
            string id,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToPage("Index");

            var idCorte = ToInt(id);
            Corte = await _mediator.Send(
                new DetallarCorteCajaQry(idCorte),
                cancellationToken);

            if (Corte == null)
                return NotFound();

            return Page();
        }

        public async Task<JsonResult> OnGetCobrosJsonAsync(
            string idCorte,
            int draw,
            int start,
            int length,
            string? search,
            string? sortBy,
            bool sortDesc,
            CancellationToken cancellationToken)
        {
            var idInt = ToInt(idCorte);
            var filter = new FilterGrd
            {
                Search = search,
                Page = length > 0 ? (start / length) + 1 : 1,
                PageSize = length > 0 ? length : 10,
                SortBy = sortBy,
                SortDesc = sortDesc
            };

            var resultado = await _mediator.Send(
                new ListarCobrosDeCajaQry(idInt, filter),
                cancellationToken);

            return new JsonResult(new DataTableResponseDto<ListarCobroCajaDto>
            {
                Draw = draw,
                RecordsTotal = resultado.TotalItems,
                RecordsFiltered = resultado.TotalItems,
                Data = resultado.Items
            });
        }

        public async Task<JsonResult> OnGetGastosJsonAsync(
            string idCorte,
            int draw,
            int start,
            int length,
            string? search,
            string? sortBy,
            bool sortDesc,
            CancellationToken cancellationToken)
        {
            var idInt = ToInt(idCorte);
            var filter = new FilterGrd
            {
                Search = search,
                Page = length > 0 ? (start / length) + 1 : 1,
                PageSize = length > 0 ? length : 10,
                SortBy = sortBy,
                SortDesc = sortDesc
            };

            var resultado = await _mediator.Send(
                new ListarGastosDeCajaQry(idInt, filter),
                cancellationToken);

            return new JsonResult(new DataTableResponseDto<ListarGastoCajaDto>
            {
                Draw = draw,
                RecordsTotal = resultado.TotalItems,
                RecordsFiltered = resultado.TotalItems,
                Data = resultado.Items
            });
        }

        public async Task<JsonResult> OnGetPagosJsonAsync(
            string idCorte,
            int draw,
            int start,
            int length,
            string? search,
            string? sortBy,
            bool sortDesc,
            CancellationToken cancellationToken)
        {
            var idInt = ToInt(idCorte);
            var filter = new FilterGrd
            {
                Search = search,
                Page = length > 0 ? (start / length) + 1 : 1,
                PageSize = length > 0 ? length : 10,
                SortBy = sortBy,
                SortDesc = sortDesc
            };

            var resultado = await _mediator.Send(
                new ListarPagosDeCajaQry(idInt, filter),
                cancellationToken);

            return new JsonResult(new DataTableResponseDto<ListarPagoCajaDto>
            {
                Draw = draw,
                RecordsTotal = resultado.TotalItems,
                RecordsFiltered = resultado.TotalItems,
                Data = resultado.Items
            });
        }
    }
}