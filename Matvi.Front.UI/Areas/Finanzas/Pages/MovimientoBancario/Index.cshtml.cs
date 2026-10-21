using System.Threading;
using System.Threading.Tasks;
using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Dtos;
using Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Dto;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Finanzas.Pages.MovimientoBancario
{
    [AuthorizeMatvi(Funcion.Finanzas_Movimientos_Bancarios)]
    public class IndexModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService
    ) : BaseModel
    {
        public void OnGet()
        {
        }

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

            byte? sucursalId = _usuarioActualService.EstaSucursalEnSesionYRelacionada == true
                ? _usuarioActualService.UsuarioSesion!.SucursalSesion!.Id
                : null;

            var resultado = await _mediator.Send(
                new ListarMovimientoBancarioQry(
                    sucursalId,
                    null,
                    null,
                    null,
                    null,
                    null,
                    filter),
                cancellationToken);

            var response = new DataTableResponseDto<ListarMovimientoBancarioDto>
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
