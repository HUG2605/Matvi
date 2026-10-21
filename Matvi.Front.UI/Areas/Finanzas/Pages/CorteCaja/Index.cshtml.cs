using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Dto;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.CorteCaja
{
    [AuthorizeMatvi(Funcion.Finanzas_Cortes_de_caja)]
    public class IndexModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
    ) : PageModel
    {
        public CajaActivaDto? Caja { get; set; }

        public bool TieneSucursal =>
            _usuarioActualService.EstaSucursalEnSesionYRelacionada == true;

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            if (TieneSucursal)
            {
                Caja = await _mediator.Send(new ObtenerCajaActivaQry(
                    _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value,
                    _usuarioActualService.UsuarioSesion!.Id), cancellationToken);
            }
        }

        public async Task<JsonResult> OnGetCortesJsonAsync(
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

            var resultado = new DataGrd<ListarCorteCajaDto>();

            if (TieneSucursal)
            {
                var usuSesion = _usuarioActualService.UsuarioSesion;
                resultado = await _mediator.Send(
                    new ListarCorteCajaQry(
                        usuSesion!.SucursalSesion!.Id!.Value,
                        usuSesion.RolId ?? 0,
                        usuSesion.Id,
                        filter),
                    cancellationToken);
            }

            return new JsonResult(new DataTableResponseDto<ListarCorteCajaDto>
            {
                Draw = draw,
                RecordsTotal = resultado.TotalItems,
                RecordsFiltered = resultado.TotalItems,
                Data = resultado.Items
            });
        }
    }
}