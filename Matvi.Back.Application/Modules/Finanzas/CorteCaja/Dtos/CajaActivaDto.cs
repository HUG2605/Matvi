using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para caja activa.
    /// </summary>
public class CajaActivaDto : IIdDto<int>, IFolioDto
    {
        #region Implementacion de IIdDto

                /// <summary>
        /// Identificador único de id.
        /// </summary>
public int Id { get; set; }
                /// <summary>
        /// Identificador único de safe id.
        /// </summary>
public string SafeId => Id.GetSafeId();

        #endregion

        #region Implementacion de IFolioDto

                /// <summary>
        /// Propiedad que representa ct sucu clave.
        /// </summary>
public string CtSucu_Clave { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa numero.
        /// </summary>
public int Numero { get; set; }
                /// <summary>
        /// Propiedad que representa folio.
        /// </summary>
public string Folio => this.GetFolio(TipoConsecutivoEnm.CORTE_DE_CAJA);

        #endregion

                /// <summary>
        /// Fecha y hora correspondiente a fecha apertura.
        /// </summary>
public DateTime FechaApertura { get; set; }
                /// <summary>
        /// Propiedad que representa fondo inicial.
        /// </summary>
public decimal FondoInicial { get; set; }
    }
}