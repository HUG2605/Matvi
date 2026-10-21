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
    [AuthorizeMatvi(Funcion.Configuracion_Empresa, Accion.Editar)]
    public class EditModel(
        IMediator _mediator
        ) : BaseModel
    {
        [BindProperty]
        public required EditarEmpresaDto Empresa { get; set; }

        [BindProperty]
        [Display(Name = "Logotipo")]
        [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Solo se permiten archivos con extensión .jpg")]
        public IFormFile? LogotipoFld { get; set; }

        public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idEmpresa = ToByte(id);
            Empresa = await _mediator.Send(
                new EditarEmpresaQry(idEmpresa), 
                cancellationToken);
        }

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