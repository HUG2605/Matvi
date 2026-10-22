using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Infrastructure.Common.Attributes
{
    /// <summary>
    /// Atributo para marcar manejadores de comandos (Command Handlers) que deben ejecutarse dentro de una transacción explícita de base de datos.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class TransactionAttribute : Attribute
    {
    }

}
