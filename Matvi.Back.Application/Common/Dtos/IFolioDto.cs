using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Common.Dtos
{
    public interface IFolioDto
    {
        string CtSucu_Clave { get; set; }

        int Numero { get; set; }

        string Folio { get; }

    }
}
