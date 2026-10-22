using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Compras.Compra.Dtos;
using Matvi.Back.Application.Modules.Compras.Compra.Queries;
using Matvi.Back.Application.Modules.Finanzas.Pago.Dtos;
using Matvi.Back.Application.Modules.Finanzas.Pago.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Dto;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.Pago
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para index.
    /// </summary>
[AuthorizeMatvi(Funcion.Finanzas_Pagos)]
    public class IndexModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ):PageModel
    {
                /// <inheritdoc/>
public void OnGet()
        {
        }
                /// <summary>
        /// Ejecuta la operación de on get pagos json async.
        /// </summary>
        /// <param name="draw">Parámetro draw.</param>
        /// <param name="start">Parámetro start.</param>
        /// <param name="length">Parámetro length.</param>
        /// <param name="search">Parámetro search.</param>
        /// <param name="sortBy">Parámetro sort by.</param>
        /// <param name="sortDesc">Parámetro sort desc.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnGetPagosJsonAsync(
       int draw,
       int start,
       int length,
       string? search,
       string? sortBy,
       bool sortDesc,
       CancellationToken cancellationToken)
        {
            var filter = new FilterGrd
            {
                Search = search,
                Page = length > 0 ? (start / length) + 1 : 1,
                PageSize = length > 0 ? length : 10,
                SortBy = sortBy,
                SortDesc = sortDesc
            };

            var resultado = new DataGrd<ListarPagoDto>();

            if (_usuarioActualService.EstaSucursalEnSesionYRelacionada == true)
            {
                resultado = await _mediator.Send(
                    new ListarPagoQry(
                        _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value,
                        filter),
                    cancellationToken);
            }

            var response = new DataTableResponseDto<ListarPagoDto>
            {
                Draw = draw,
                RecordsTotal = resultado.TotalItems,
                RecordsFiltered = resultado.TotalItems,
                Data = resultado.Items
            };

            return new JsonResult(response);
        }
    }
}
