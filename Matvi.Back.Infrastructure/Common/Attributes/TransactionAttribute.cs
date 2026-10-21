using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Infrastructure.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class TransactionAttribute : Attribute
    {
    }

}
