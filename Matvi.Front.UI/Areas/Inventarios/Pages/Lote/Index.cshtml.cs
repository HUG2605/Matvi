using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Inventarios.Lote.Dtos;
using Matvi.Back.Application.Modules.Inventarios.Lote.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Dto;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Matvi.Front.UI.Areas.Inventarios.Pages.Lotes
{
    [AuthorizeMatvi(Funcion.Inventarios_Lotes)]
    public class IndexModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ) : BaseModel
    {
        public void OnGet()
        {

        }

        public async Task<JsonResult> OnGetLotesJsonAsync(
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

            var resultado = new DataGrd<ListarLoteDto>();

            if (_usuarioActualService.EstaSucursalEnSesionYRelacionada == true) 
            {
                 resultado = await _mediator.Send(
                     new ListarLoteQry(
                         _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value, 
                         filter), 
                     cancellationToken); 
            }
              
            var response = new DataTableResponseDto<ListarLoteDto>
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