using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Common.Dtos
{
    public interface IPresentacionProductoBaseDto
    {
        int CtPresProd_IdCtProducto { get; set; }
        string CtPresProd_Nombre { get; set; }
        decimal CtPresProd_FactorUBI { get; set; }
    }
}
