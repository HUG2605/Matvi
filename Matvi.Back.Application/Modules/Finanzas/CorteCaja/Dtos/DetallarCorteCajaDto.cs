using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Extensions;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para detallar corte caja.
    /// </summary>
public class DetallarCorteCajaDto : IIdDto<int>, IFolioDto
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
        /// Texto descriptivo o detalle de ct sucu nombre.
        /// </summary>
public string CtSucu_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Texto descriptivo o detalle de ct usua nombre.
        /// </summary>
public string CtUsua_Nombre { get; set; } = string.Empty;
                /// <summary>
        /// Fecha y hora correspondiente a fecha apertura.
        /// </summary>
public DateTime FechaApertura { get; set; }
                /// <summary>
        /// Fecha y hora correspondiente a fecha cierre.
        /// </summary>
public DateTime? FechaCierre { get; set; }
                /// <summary>
        /// Propiedad que representa fondo inicial.
        /// </summary>
public decimal FondoInicial { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total cobros.
        /// </summary>
public decimal? TotalCobros { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total cobros efectivo.
        /// </summary>
public decimal? TotalCobrosEfectivo { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total cobros bancario.
        /// </summary>
public decimal? TotalCobrosBancario { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total entregas repartidor.
        /// </summary>
public decimal? TotalEntregasRepartidor { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total gastos.
        /// </summary>
public decimal? TotalGastos { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total gastos efectivo.
        /// </summary>
public decimal? TotalGastosEfectivo { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total gastos bancario.
        /// </summary>
public decimal? TotalGastosBancario { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total pagos.
        /// </summary>
public decimal? TotalPagos { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total pagos efectivo.
        /// </summary>
public decimal? TotalPagosEfectivo { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total pagos bancario.
        /// </summary>
public decimal? TotalPagosBancario { get; set; }
                /// <summary>
        /// Propiedad que representa fondo final sistema.
        /// </summary>
public decimal? FondoFinalSistema { get; set; }
                /// <summary>
        /// Propiedad que representa fondo final contado.
        /// </summary>
public decimal? FondoFinalContado { get; set; }
                /// <summary>
        /// Propiedad que representa diferencia.
        /// </summary>
public decimal? Diferencia { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total ventas.
        /// </summary>
public decimal? TotalVentas { get; set; }
                /// <summary>
        /// Monto económico correspondiente a costo ventas real.
        /// </summary>
public decimal? CostoVentasReal { get; set; }
                /// <summary>
        /// Monto económico correspondiente a costo ventas regularizado.
        /// </summary>
public decimal? CostoVentasRegularizado { get; set; }
                /// <summary>
        /// Propiedad que representa ventas sin costear.
        /// </summary>
public decimal? VentasSinCostear { get; set; }
                /// <summary>
        /// Propiedad que representa utilidad bruta.
        /// </summary>
public decimal? UtilidadBruta { get; set; }
                /// <summary>
        /// Propiedad que representa utilidad neta.
        /// </summary>
public decimal? UtilidadNeta { get; set; }
                /// <summary>
        /// Propiedad que representa porcentaje ventas sin costear.
        /// </summary>
public decimal? PorcentajeVentasSinCostear { get; set; }
                /// <summary>
        /// Propiedad que representa porcentaje cobertura gastos.
        /// </summary>
public decimal? PorcentajeCoberturaGastos { get; set; }
                /// <summary>
        /// Propiedad que representa gastos absorbidos de capital.
        /// </summary>
public decimal? GastosAbsorbidosDeCapital { get; set; }
                /// <summary>
        /// Propiedad que representa notas.
        /// </summary>
public string? Notas { get; set; }
    }
}