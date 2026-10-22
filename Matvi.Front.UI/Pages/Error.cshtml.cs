using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Matvi.Front.UI.Pages
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para error.
    /// </summary>
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
                /// <summary>
        /// Propiedad que representa error code.
        /// </summary>
public string? ErrorCode { get; set; }

                /// <inheritdoc/>
public void OnGet(string errorCode)
        {
            ErrorCode = errorCode;
        }
    }
}
