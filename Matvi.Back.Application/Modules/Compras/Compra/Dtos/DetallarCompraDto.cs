using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Compras.Compra.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para detallar compra.
    /// </summary>
public class DetallarCompraDto : IIdDto<int>, IFolioDto
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

        #region Generales

                /// <summary>
        /// Texto descriptivo o detalle de ct sucu nombre.
        /// </summary>
public string CtSucu_Nombre { get; set; }= string.Empty;
                /// <summary>
        /// Fecha y hora correspondiente a fecha.
        /// </summary>
public DateTime Fecha { get; set; }
                /// <summary>
        /// Texto descriptivo o detalle de ct prov nombre.
        /// </summary>
public string CtProv_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Monto económico correspondiente a total.
        /// </summary>
public decimal Total { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total pagado.
        /// </summary>
public decimal TotalPagado { get; set; }
                /// <summary>
        /// Monto económico correspondiente a saldo pendiente.
        /// </summary>
public decimal SaldoPendiente => Total - TotalPagado;
                /// <summary>
        /// Indica si estatus pago se encuentra activo o habilitado.
        /// </summary>
public string EstatusPago => TotalPagado >= Total ? "PAGADA" : "PENDIENTE";

        #endregion

        #region Productos

                /// <summary>
        /// Colección de elementos asociados a productos.
        /// </summary>
public List<DetallarCompraDetalleDto> Productos { get; set; } = [];

        #endregion

        #region Lotes

                /// <summary>
        /// Colección de elementos asociados a lotes.
        /// </summary>
public List<DetallarCompraLoteDto> Lotes { get; set; } = [];

        #endregion

        #region Pagos

                /// <summary>
        /// Colección de elementos asociados a pagos.
        /// </summary>
public List<DetallarCompraPagoDto> Pagos { get; set; } = [];
        
        #endregion

    }
}
