namespace Matvi.Back.Application.Common.Enums
{
    /// <summary>
    /// Estatus visible a nivel header del pedido. Coincide 1:1 con CtEstatusPedido.
    /// </summary>
    public enum EstatusPedidoEnm : byte
    {
        /// <summary>
        /// Pedido recién capturado; ninguna línea se ha empezado a surtir.
        /// </summary>
        CREADO = 1,

        /// <summary>
        /// Pedido pagado al 100% al momento de la captura; el cliente lo recogerá o recibirá
        /// posteriormente. No valida ni descuenta inventario mientras permanezca aquí.
        /// Exclusivo para clientes registrados.
        /// </summary
        /// >
        APARTADO = 2,

        /// <summary>
        /// Al menos una línea ya se encuentra en movimiento (reservada, surtida, en reparto
        /// o entregada). Agrupa las etapas operativas del surtido y entrega del pedido.
        /// </summary>
        EN_PROCESO = 3,

        /// <summary>
        /// El 100% de las líneas del pedido fue entregado completamente al cliente.
        /// </summary>
        ENTREGADO = 4,

        /// <summary>
        /// El pedido se cerró con al menos una línea no entregada en su totalidad, ya sea
        /// por rechazo del cliente o por cierre administrativo de productos no surtidos.
        /// </summary>
        ENTREGADO_PARCIAL = 5,

        /// <summary>
        /// El cliente rechazó la totalidad del pedido o ninguna línea fue entregada antes del cierre.
        /// </summary>
        RECHAZADO = 6,

        /// <summary>
        /// El pedido fue cancelado antes de que cualquier línea fuera surtida.
        /// </summary>
        CANCELADO = 7
    }
}
