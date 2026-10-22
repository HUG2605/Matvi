using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Notifications.WebPush.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para web push keys.
    /// </summary>
public class WebPushKeysDto
    {
                /// <summary>
        /// Propiedad que representa p 256 dh.
        /// </summary>
public string P256dh { get; set; } = null!;
                /// <summary>
        /// Propiedad que representa auth.
        /// </summary>
public string Auth { get; set; } = null!;
    }
}
