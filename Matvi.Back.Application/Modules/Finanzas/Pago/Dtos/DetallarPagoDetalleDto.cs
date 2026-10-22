using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Finanzas.Pago.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para detallar pago detalle.
    /// </summary>
public class DetallarPagoDetalleDto : IIdDto<int>, IFolioDto
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
public string Folio => this.GetFolio(TipoConsecutivoEnm.COMPRA);
        #endregion

                /// <summary>
        /// Fecha y hora correspondiente a fecha compra.
        /// </summary>
public DateTime FechaCompra { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total compra.
        /// </summary>
public decimal TotalCompra { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total pagado.
        /// </summary>
public decimal TotalPagado { get; set; }
                /// <summary>
        /// Monto económico correspondiente a monto aplicado.
        /// </summary>
public decimal MontoAplicado { get; set; }
                /// <summary>
        /// Monto económico correspondiente a saldo pendiente.
        /// </summary>
public decimal SaldoPendiente => TotalCompra - TotalPagado;
    }
}
