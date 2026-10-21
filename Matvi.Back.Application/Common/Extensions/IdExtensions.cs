using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Common.Extensions
{
    public static class IdExtensions
    {
        public static string GetSafeId<TId>(this TId id)
            => id?.ToString()?.ToObfuscated() ?? string.Empty;
    }
}
