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
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para details.
    /// </summary>
[AuthorizeMatvi(Funcion.Finanzas_Cortes_de_caja)]
    public class DetailsModel(
        IMediator _mediator
    ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa corte.
        /// </summary>
public DetallarCorteCajaDto? Corte { get; set; }

                /// <inheritdoc/>
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

                /// <summary>
        /// Ejecuta la operación de on get cobros json async.
        /// </summary>
        /// <param name="idCorte">Identificador único del registro u objeto.</param>
        /// <param name="draw">Parámetro draw.</param>
        /// <param name="start">Parámetro start.</param>
        /// <param name="length">Parámetro length.</param>
        /// <param name="search">Parámetro search.</param>
        /// <param name="sortBy">Parámetro sort by.</param>
        /// <param name="sortDesc">Parámetro sort desc.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
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

                /// <summary>
        /// Ejecuta la operación de on get gastos json async.
        /// </summary>
        /// <param name="idCorte">Identificador único del registro u objeto.</param>
        /// <param name="draw">Parámetro draw.</param>
        /// <param name="start">Parámetro start.</param>
        /// <param name="length">Parámetro length.</param>
        /// <param name="search">Parámetro search.</param>
        /// <param name="sortBy">Parámetro sort by.</param>
        /// <param name="sortDesc">Parámetro sort desc.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
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

                /// <summary>
        /// Ejecuta la operación de on get pagos json async.
        /// </summary>
        /// <param name="idCorte">Identificador único del registro u objeto.</param>
        /// <param name="draw">Parámetro draw.</param>
        /// <param name="start">Parámetro start.</param>
        /// <param name="length">Parámetro length.</param>
        /// <param name="search">Parámetro search.</param>
        /// <param name="sortBy">Parámetro sort by.</param>
        /// <param name="sortDesc">Parámetro sort desc.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
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