using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Common
{
    public class WarmUp
    {

        public static async Task InitializeAsync(IServiceScope scope)
        {
            await Infrastructure.Performance.WarmUpDbContext.InitializeAsync(scope);
        }
    }
}
