using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Compras.Compra.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para detallar compra pago.
    /// </summary>
public class DetallarCompraPagoDto:IIdDto<int>, IFolioDto
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
public string Folio => this.GetFolio(TipoConsecutivoEnm.PAGO);

        #endregion

                /// <summary>
        /// Fecha y hora correspondiente a fecha.
        /// </summary>
public DateTime Fecha { get; set; }
                /// <summary>
        /// Monto económico correspondiente a monto total.
        /// </summary>
public decimal MontoTotal { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de ct medi pago nombre.
        /// </summary>
public string CtMediPago_Nombre { get; set; }=string.Empty;
                /// <summary>
        /// Texto descriptivo o detalle de ct usua paga nombre.
        /// </summary>
public string CtUsuaPaga_Nombre { get; set; }=string.Empty;
                /// <summary>
        /// Monto económico correspondiente a monto aplicado.
        /// </summary>
public decimal MontoAplicado { get; set; }
                /// <summary>
        /// Monto económico correspondiente a saldo inicial.
        /// </summary>
public decimal SaldoInicial { get; set; }
                /// <summary>
        /// Monto económico correspondiente a saldo final.
        /// </summary>
public decimal SaldoFinal => SaldoInicial - MontoAplicado;
    }
}
