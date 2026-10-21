using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Matvi.Back.Application.Common.Constants
{
    internal static class RegExp
    {
        public const string Numerico = @"^\d+$";

        public const string Alfabetico = @"^(?!.*[.]{2})(?!.*\s{2})[A-Za-zÁÉÍÓÚÜáéíóúüÑñ]+([.\s]?[A-Za-zÁÉÍÓÚÜáéíóúüÑñ]+)*$";

        public const string Decimal = @"^\d+(\.\d{1,2})?$";

        public const string RFCPattern = @"^([A-ZÑ&]{3,4})\d{6}[A-Z0-9]{3}$";


    }

}
