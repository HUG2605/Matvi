using Matvi.Front.UI.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Matvi.Front.UI.Pages.Security
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para logout.
    /// </summary>
public class LogoutModel : PageModel
    {
                /// <inheritdoc/>
public async Task<IActionResult> OnGetAsync()
        {
            await HttpContext.SignOutAsync(AuthConstants.Scheme);
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
    }
}
