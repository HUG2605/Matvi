using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Dtos;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Queries;
using Matvi.Back.Application.Modules.Finanzas.Pago.Dtos;
using Matvi.Back.Application.Modules.Finanzas.Pago.Queries;
using Matvi.Front.UI.Dto;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.Gasto
{
    public class IndexModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ) : PageModel
    {
        public void OnGet()
        {
        }
        public async Task<JsonResult> OnGetGastosJsonAsync(
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

            var resultado = new DataGrd<ListarGastoDto>();

            if (_usuarioActualService.EstaSucursalEnSesionYRelacionada == true)
            {
                resultado = await _mediator.Send(
                    new ListarGastoQry(
                        _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value,
                        filter),
                    cancellationToken);
            }

            var response = new DataTableResponseDto<ListarGastoDto>
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
