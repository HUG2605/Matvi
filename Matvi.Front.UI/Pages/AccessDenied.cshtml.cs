using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Matvi.Front.UI.Pages
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para access denied.
    /// </summary>
[AllowAnonymous]
    public class AccessDeniedModel : PageModel
    {
                /// <inheritdoc/>
public void OnGet()
        {
            //  AccessDenied
        }
    }
}
