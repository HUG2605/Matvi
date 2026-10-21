using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Common.Grid
{
    public class DataGrd<T>
    {
        public int TotalItems { get; set; }
        public IReadOnlyList<T> Items { get; set; } = [];
    }

}
