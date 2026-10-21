using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Compras.Compra.Dtos;
using Matvi.Back.Application.Modules.Compras.Compra.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Dto;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Matvi.Front.UI.Areas.Compras.Pages.Compras
{
    [AuthorizeMatvi(Funcion.Compras_Compras)]
    public class IndexModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ) : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<JsonResult> OnGetComprasJsonAsync(
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

            var resultado = new DataGrd<ListarCompraDto>();

            if (_usuarioActualService.EstaSucursalEnSesionYRelacionada == true)
            {
                resultado = await _mediator.Send(
                    new ListarCompraQry(
                        _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value,
                        filter) , 
                    cancellationToken);
            }

            var response = new DataTableResponseDto<ListarCompraDto>
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
