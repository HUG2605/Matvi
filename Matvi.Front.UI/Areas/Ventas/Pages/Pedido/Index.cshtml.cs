using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using Matvi.Back.Application.Modules.Ventas.Pedido.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Dto;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Matvi.Front.UI.Areas.Ventas.Pages.Pedido
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para index.
    /// </summary>
[AuthorizeMatvi(Funcion.Ventas_Pedidos)]
    public class IndexModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ) : PageModel
    {
                /// <inheritdoc/>
public void OnGet()
        {
        }

                /// <summary>
        /// Ejecuta la operación de on get pedidos json async.
        /// </summary>
        /// <param name="draw">Parámetro draw.</param>
        /// <param name="start">Parámetro start.</param>
        /// <param name="length">Parámetro length.</param>
        /// <param name="search">Parámetro search.</param>
        /// <param name="sortBy">Parámetro sort by.</param>
        /// <param name="sortDesc">Parámetro sort desc.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnGetPedidosJsonAsync(
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

            var resultado = new DataGrd<ListarPedidoDto>();

            var puedeVerSensibles = _usuarioActualService
            .PuedeVerSensibles(Funcion.Ventas_Pedidos);

            if (_usuarioActualService.EstaSucursalEnSesionYRelacionada == true)
            {
                resultado = await _mediator.Send(
                    new ListarPedidoQry(
                        _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value,
                        _usuarioActualService.UsuarioSesion!.RolId!.Value,
                        _usuarioActualService.UsuarioSesion!.Id,
                        puedeVerSensibles,
                        filter),
                    cancellationToken);
            }

            return new JsonResult(new DataTableResponseDto<ListarPedidoDto>
            {
                Draw = draw,
                RecordsTotal = resultado.TotalItems,
                RecordsFiltered = resultado.TotalItems,
                Data = resultado.Items
            });
        }

                /// <summary>
        /// Ejecuta la operación de on get estado credito cliente async.
        /// </summary>
        /// <param name="idCliente">Identificador único del registro u objeto.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnGetEstadoCreditoClienteAsync(
            int idCliente,
            CancellationToken cancellationToken)
        {
            var estadoCredito = await _mediator.Send(
                new Matvi.Back.Application.Modules.Ventas.Cliente.Queries.ObtenerEstadoCreditoClienteQry(idCliente),
                cancellationToken);

            return new JsonResult(estadoCredito);
        }
    }
}