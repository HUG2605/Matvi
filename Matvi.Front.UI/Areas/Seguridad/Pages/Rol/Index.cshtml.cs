using Matvi.Back.Application.Modules.Seguridad.Rol.Dtos;
using Matvi.Back.Application.Modules.Seguridad.Rol.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;

namespace Matvi.Front.UI.Areas.Seguridad.Pages.Rol
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para index.
    /// </summary>
[AuthorizeMatvi(Funcion.Seguridad_Roles)]
    public class IndexModel(
        IMediator _mediator
        ) : BaseModel
    {
                /// <summary>
        /// Colección de elementos asociados a roles.
        /// </summary>
public List<ListarRolDto>? Roles { get; set; }

                /// <inheritdoc/>
public async Task OnGet(
            CancellationToken cancellationToken)
        {
            Roles = await _mediator.Send(
                new ListarRolQry(), 
                cancellationToken);
        }
    }
}
