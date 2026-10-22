using Matvi.Back.Application.Modules.Configuracion.Empresa.Commands;
using Matvi.Back.Application.Modules.Configuracion.Empresa.Dtos;
using Matvi.Back.Application.Modules.Configuracion.Empresa.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Front.UI.Areas.Configuracion.Pages.Empresa
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para edit.
    /// </summary>
[AuthorizeMatvi(Funcion.Configuracion_Empresa, Accion.Editar)]
    public class EditModel(
        IMediator _mediator
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa empresa.
        /// </summary>
[BindProperty]
        public required EditarEmpresaDto Empresa { get; set; }

                /// <summary>
        /// Propiedad que representa logotipo.
        /// </summary>
[BindProperty]
        [Display(Name = "Logotipo")]
        [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Solo se permiten archivos con extensión .jpg")]
        public IFormFile? LogotipoFld { get; set; }

                /// <inheritdoc/>
public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idEmpresa = ToByte(id);
            Empresa = await _mediator.Send(
                new EditarEmpresaQry(idEmpresa), 
                cancellationToken);
        }

                /// <summary>
        /// Ejecuta la operación de on post edit async.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnPostEditAsync(
            CancellationToken cancellationToken)
        {
            Empresa.Id = ToByte(Request.Form["Empresa.SafeId"]!);
            if (LogotipoFld != null && LogotipoFld.Length > 0)
            {
                using var ms = new MemoryStream();
                await LogotipoFld.CopyToAsync(ms, cancellationToken);
                Empresa.LogotipoFld = Convert.ToBase64String(ms.ToArray());
            }

            var res = await _mediator.Send(
                new EditarEmpresaCmd(Empresa), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}