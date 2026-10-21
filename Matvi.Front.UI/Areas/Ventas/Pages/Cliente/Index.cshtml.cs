using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Ventas.Cliente.Dtos;
using Matvi.Back.Application.Modules.Ventas.Cliente.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Dto;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Matvi.Front.UI.Areas.Ventas.Pages.Cliente
{
    [AuthorizeMatvi(Funcion.Ventas_Clientes)]
    public class IndexModel(
        IMediator _mediator
        ) : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<JsonResult> OnGetClientesJsonAsync(
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

             var resultado = await _mediator.Send(
                 new ListarClienteQry(filter), 
                 cancellationToken);
           
            var response = new DataTableResponseDto<ListarClienteDto>
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
