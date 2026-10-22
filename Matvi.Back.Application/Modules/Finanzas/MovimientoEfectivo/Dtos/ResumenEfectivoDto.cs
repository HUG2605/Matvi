namespace Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Dtos
{
        /// <summary>
    /// Objeto de transferencia de datos (DTO) para resumen efectivo.
    /// </summary>
public class ResumenEfectivoDto
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
        /// Propiedad que representa flujo neto.
        /// </summary>
public decimal FlujoNeto => TotalIngresos - TotalEgresos;
                /// <summary>
        /// Monto económico correspondiente a total sin corte.
        /// </summary>
public int TotalSinCorte { get; set; }
                /// <summary>
        /// Monto económico correspondiente a monto sin corte.
        /// </summary>
public decimal MontoSinCorte { get; set; }
                /// <summary>
        /// Monto económico correspondiente a total movimientos.
        /// </summary>
public int TotalMovimientos { get; set; }
    }
}
