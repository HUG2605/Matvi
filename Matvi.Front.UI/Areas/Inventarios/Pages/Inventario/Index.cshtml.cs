using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Dtos;
using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Dto;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Inventarios.Pages.Inventario
{
    [AuthorizeMatvi(Funcion.Inventarios_Inventario)]
    public class IndexModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
        ) : BaseModel
    {
        public void OnGet()
        {
        }

        public async Task<JsonResult> OnGetInventariosJsonAsync(
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

            var resultado = new DataGrd<ListarProductoSucursalDto>();

            if (_usuarioActualService.EstaSucursalEnSesionYRelacionada == true)
            {
                resultado = await _mediator.Send(
                    new ListarProductoSucursalQry(
                        _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id!.Value,
                        filter),
                    cancellationToken);
            }

            var response = new DataTableResponseDto<ListarProductoSucursalDto>
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
