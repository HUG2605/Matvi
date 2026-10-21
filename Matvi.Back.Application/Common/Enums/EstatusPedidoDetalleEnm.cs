namespace Matvi.Back.Application.Common.Enums
{
    /// <summary>
    /// Estatus cacheado por línea de pedido. Coincide 1:1 con CtEstatusPedidoDetalle.
    /// </summary>
    public enum EstatusPedidoDetalleEnm : byte
    {
        /// <summary>
        /// Línea recién capturada sin cantidad apartada o surtida. Aún no ha sido
        /// atendida o no existe inventario suficiente para procesarla. Algunas líneas
        /// pueden omitir este estatus si se entregan directamente al momento de su captura.
        /// </summary>
        PENDIENTE = 1,

        /// <summary>
        /// La línea ya tiene actividad operativa: existe cantidad reservada, surtida
        /// parcial o totalmente, o se encuentra bajo custodia del repartidor. Los eventos
        /// específicos se registran en OpPedidoDetalleEvento.
        /// </summary>
        EN_PROCESO = 2,

        /// <summary>
        /// La línea ya no tiene cantidad pendiente por resolver. Toda la cantidad solicitada
        /// fue distribuida entre CantidadEntregada y CantidadRechazada, existiendo al menos
        /// una operación de surtido, entrega o rechazo.
        /// </summary>
        CERRADO = 3,

        /// <summary>
        /// La línea fue cancelada antes de cualquier afectación al inventario; no existió
        /// surtido ni movimiento de mercancía.
        /// </summary>
        CANCELADO = 4
    }
}