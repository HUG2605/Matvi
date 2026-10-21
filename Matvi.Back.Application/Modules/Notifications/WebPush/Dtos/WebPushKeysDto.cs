using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Notifications.WebPush.Dtos
{
    public class WebPushKeysDto
    {
        public string P256dh { get; set; } = null!;
        public string Auth { get; set; } = null!;
    }
}
