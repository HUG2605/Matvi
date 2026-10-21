using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Notifications.WebPush.Dtos
{
    public class WebPushSuscripcionDto
    {
        public short CtUsua_Id { get; set; }
        public string Endpoint { get; set; } = null!;
        public WebPushKeysDto Keys { get; set; } = null!;
    }
}
