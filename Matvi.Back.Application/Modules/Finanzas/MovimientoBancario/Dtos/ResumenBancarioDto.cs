namespace Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para resumen bancario.
    /// </summary>
public class ResumenBancarioDto
    {
                /// <summary>
        /// Monto económico correspondiente a total ingresos.
        /// </summary>
public decimal TotalIngresos { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total egresos.
        /// </summary>
public decimal TotalEgresos { get; set; }
                /// <summary>
        /// Propiedad que representa balance neto.
        /// </summary>
public decimal BalanceNeto => TotalIngresos - TotalEgresos;
                /// <summary>
        /// Monto económico correspondiente a total pendientes conciliar.
        /// </summary>
public int TotalPendientesConciliar { get; set; }
                /// <summary>
        /// Monto económico correspondiente a monto pendiente conciliar.
        /// </summary>
public decimal MontoPendienteConciliar { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total movimientos.
        /// </summary>
public int TotalMovimientos { get; set; }
    }
}
