using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Common.Dtos
{
        /// <summary>
    /// Contrato que define las operaciones de folio.
    /// </summary>
public interface IFolioDto
    {
        /// <summary>
        /// Clave o código identificador de la sucursal.
        /// </summary>
        string CtSucu_Clave { get; set; }

        /// <summary>
        /// Número secuencial consecutivo del documento.
        /// </summary>
        int Numero { get; set; }

        /// <summary>
        /// Folio formateado completo del documento.
        /// </summary>
        string Folio { get; }

    }
}
