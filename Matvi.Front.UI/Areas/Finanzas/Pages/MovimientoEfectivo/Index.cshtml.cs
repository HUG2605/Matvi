using System.Threading;
using System.Threading.Tasks;
using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Dtos;
using Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Dto;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.MovimientoEfectivo
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para index.
    /// </summary>
[AuthorizeMatvi(Funcion.Finanzas_Movimientos_Efectivo)]
    public class IndexModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
    ) : BaseModel
    {
                /// <inheritdoc/>
public void OnGet()
        {
        }

                /// <summary>
        /// Ejecuta la operación de on get movimientos json async.
        /// </summary>
        /// <param name="draw">Parámetro draw.</param>
        /// <param name="start">Parámetro start.</param>
        /// <param name="length">Parámetro length.</param>
        /// <param name="search">Parámetro search.</param>
        /// <param name="sortBy">Parámetro sort by.</param>
        /// <param name="sortDesc">Parámetro sort desc.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnGetMovimientosJsonAsync(
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

            var resultado = new DataGrd<ListarMovimientoEfectivoDto>();

            if (_usuarioActualService.EstaSucursalEnSesionYRelacionada == true)
            {
                var sucursalId = _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value;
                resultado = await _mediator.Send(
                    new ListarMovimientoEfectivoQry(
                        sucursalId,
                        null,
                        null,
                        null,
                        null,
                        null,
                        filter),
                    cancellationToken);
            }

            var response = new DataTableResponseDto<ListarMovimientoEfectivoDto>
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
