using Matvi.Back.Application.Modules.Configuracion.Empresa.Dtos;
using Matvi.Back.Application.Modules.Configuracion.Empresa.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;

namespace Matvi.Front.UI.Areas.Configuracion.Pages.Empresa
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para details.
    /// </summary>
[AuthorizeMatvi(Funcion.Configuracion_Empresa)]
    public class DetailsModel(
        IMediator _mediator
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa empresa.
        /// </summary>
public DetallarEmpresaDto? Empresa { get; set; }

                /// <inheritdoc/>
public async Task OnGet(
            string id, 
            CancellationToken cancellationToken)
        {
            var idEmpresa = ToByte(id);
            Empresa = await _mediator.Send(
                new DetallarEmpresaQry(idEmpresa), 
                cancellationToken);
        }

    }
}
