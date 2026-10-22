using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Common
{
        /// <summary>
    /// Representa la clase para warm up.
    /// </summary>
public class WarmUp
    {

                /// <summary>
        /// Ejecuta la operación de initialize async.
        /// </summary>
        /// <param name="scope">Parámetro scope.</param>
public static async Task InitializeAsync(IServiceScope scope)
        {
            await Infrastructure.Performance.WarmUpDbContext.InitializeAsync(scope);
        }
    }
}
