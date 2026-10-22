using System;
using System.Collections.Generic;
using System.Text;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para cerrar corte caja res.
    /// </summary>
public class CerrarCorteCajaResDto
    {
                /// <summary>
        /// Monto económico correspondiente a total cobros.
        /// </summary>
public decimal TotalCobros { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total entregas repartidor.
        /// </summary>
public decimal TotalEntregasRepartidor { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total gastos.
        /// </summary>
public decimal TotalGastos { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total pagos.
        /// </summary>
public decimal TotalPagos { get; set; }
                /// <summary>
        /// Propiedad que representa fondo final sistema.
        /// </summary>
public decimal FondoFinalSistema { get; set; }
                /// <summary>
        /// Propiedad que representa fondo final contado.
        /// </summary>
public decimal FondoFinalContado { get; set; }
                /// <summary>
        /// Propiedad que representa diferencia.
        /// </summary>
public decimal Diferencia { get; set; }
    }
}