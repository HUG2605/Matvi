namespace Matvi.Back.Application.Common.Enums
{
    /// <summary>
    /// Tipos de evento registrados en el historial de cada línea de pedido (OpPedidoDetalleEvento).
    /// Coincide 1:1 con CtTipoEventoPedidoDetalle (5 filas en BD).
    ///
    /// Nota: el estatus de línea EN_PROCESO agrupa operativamente los eventos
    /// RESERVADO y SURTIDO; este enum permite distinguir cada paso en el historial.
    /// El receptor del surtido (repartidor o cliente directo) queda en
    /// OpPedidoDetalleEvento.IdCtUsuarioRecibe del evento SURTIDO.
    /// </summary>
    public enum TipoEventoPedidoDetalleEnm : byte
    {
        /// <summary>
        /// Se reservó inventario para la línea sin generar movimiento físico todavía.
        /// </summary>
        RESERVADO = 1,

        /// <summary>
        /// Se generó movimiento de salida de inventario para la línea.
        /// Genera registro en OpMovimientoInventario (Tipo = SAL_VENTA).
        /// El receptor queda en IdCtUsuarioRecibe (repartidor o null para entrega directa).
        /// </summary>
        SURTIDO = 2,

        /// <summary>
        /// El cliente recibió la cantidad. Puede coexistir con RECHAZADO en la misma línea
        /// cuando hay entregas parciales.
        /// </summary>
        ENTREGADO = 3,

        /// <summary>
        /// El cliente rechazó la cantidad al momento de la entrega a domicilio.
        /// Genera registro de reingreso en OpMovimientoInventario (Tipo = ENT_REINGRESO).
        /// </summary>
        RECHAZADO = 4,

        /// <summary>
        /// La línea fue cancelada administrativamente antes de su entrega.
        /// Libera el stock reservado en RlProductoSucursal.
        /// </summary>
        CANCELADO = 5
    }
}
