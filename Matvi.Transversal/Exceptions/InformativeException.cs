using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Transversal.Exceptions
{
    public class InformativeException : Exception
    {
        public InformativeException(string message) : base(message) 
        {
        }

    }
}
