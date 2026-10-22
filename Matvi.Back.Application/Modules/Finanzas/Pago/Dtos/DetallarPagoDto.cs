using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Finanzas.Pago.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para detallar pago.
    /// </summary>
public class DetallarPagoDto : IIdDto<int>, IFolioDto
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
        /// Texto descriptivo o detalle de ct sucu nombre.
        /// </summary>
public string CtSucu_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Fecha y hora correspondiente a fecha.
        /// </summary>
public DateTime Fecha { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de ct prov nombre.
        /// </summary>
public string CtProv_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Monto económico correspondiente a monto total.
        /// </summary>
public decimal MontoTotal { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de ct medio pago nombre.
        /// </summary>
public string CtMedioPago_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Texto descriptivo o detalle de ct usuario paga nombre.
        /// </summary>
public string CtUsuarioPaga_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Propiedad que representa notas.
        /// </summary>
public string? Notas { get; set; }

                /// <summary>
        /// Colección de elementos asociados a compras.
        /// </summary>
public List<DetallarPagoDetalleDto> Compras { get; set; } = [];
    }
}