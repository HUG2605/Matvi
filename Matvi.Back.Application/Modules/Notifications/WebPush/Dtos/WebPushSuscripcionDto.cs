using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Notifications.WebPush.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para web push suscripcion.
    /// </summary>
public class WebPushSuscripcionDto
    {
                /// <summary>
        /// Identificador único de ct usua id.
        /// </summary>
public short CtUsua_Id { get; set; }
                /// <summary>
        /// Propiedad que representa endpoint.
        /// </summary>
public string Endpoint { get; set; } = null!;
                /// <summary>
        /// Propiedad que representa keys.
        /// </summary>
public WebPushKeysDto Keys { get; set; } = null!;
    }
}
