using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Compras.Proveedor.Commands;
using Matvi.Back.Application.Modules.Compras.Proveedor.Dtos;
using Matvi.Back.Application.Modules.Compras.Proveedor.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using Matvi.Front.UI.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Matvi.Front.UI.Areas.Compras.Pages.Proveedor
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para details.
    /// </summary>
[AuthorizeMatvi(Funcion.Compras_Proveedores)]
    public class DetailsModel(
        IMediator _mediator,
        IUsuarioActualService _usuarioActualService   
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa proveedor.
        /// </summary>
public DetallarProveedorDto? Proveedor { get; set; }

                /// <inheritdoc/>
public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idProveedor = ToInt(id);
            Proveedor = await _mediator.Send(
                new DetallarProveedorQry(idProveedor), 
                cancellationToken);
        }

                /// <summary>
        /// Ejecuta la operación de on get delete.
        /// </summary>
        /// <param name="id">Identificador único del registro u objeto.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnGetDelete(
            string id, 
            CancellationToken cancellationToken)
        {
            if (!_usuarioActualService.TienePermiso(
                Funcion.Compras_Proveedores,
                Accion.Eliminar))
            {
                return new JsonResult(OperationResult<bool>.Error("Acceso denegado"));
            }

            var idProveedor = ToInt(id);
            var res = await _mediator.Send(
                new EliminarProveedorCmd(idProveedor), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
