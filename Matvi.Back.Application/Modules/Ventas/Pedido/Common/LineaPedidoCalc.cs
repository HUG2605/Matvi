using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Infrastructure.Persistence.Entities;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Common
{
    /// <summary>
    /// Helper centralizado con las fórmulas de cálculo de estatus operativo
    /// para líneas de pedido (OpPedidoDetalle) y encabezados (OpPedido).
    /// </summary>
    public static class LineaPedidoCalc
    {
        /// <summary>
        /// Cantidad restante por resolver en la línea (entregar, rechazar o cancelar).
        /// porResolver = Cantidad - Entregada - Rechazada - Cancelada.
        /// </summary>
        public static decimal PorResolver(OpPedidoDetalleEtt detalle) =>
            detalle.Cantidad - detalle.CantidadEntregada - detalle.CantidadRechazada - detalle.CantidadCancelada;

        /// <summary>
        /// Sobrecarga para cálculo directo con valores decimales.
        /// </summary>
        public static decimal PorResolver(decimal cantidad, decimal entregada, decimal rechazada, decimal cancelada) =>
            cantidad - entregada - rechazada - cancelada;

        /// <summary>
        /// Determina el estatus de una línea a partir de sus contadores de caché:
        /// 1. Si Cancelada >= Cantidad -> CANCELADO.
        /// 2. Si porResolver &lt;= 0 -> CERRADO.
        /// 3. Si hay actividad en cualquier caché (Surtida, Entregada, Rechazada, Cancelada > 0) -> EN_PROCESO.
        /// 4. En cualquier otro caso -> PENDIENTE.
        /// </summary>
        public static EstatusPedidoDetalleEnm CalcularEstatusLinea(OpPedidoDetalleEtt detalle)
        {
            if (detalle.CantidadCancelada >= detalle.Cantidad)
            {
                return EstatusPedidoDetalleEnm.CANCELADO;
            }

            var porResolver = PorResolver(detalle);
            if (porResolver <= 0)
            {
                return EstatusPedidoDetalleEnm.CERRADO;
            }

            var hayActividad =
                detalle.CantidadSurtida > 0
                || detalle.CantidadCancelada > 0
                || detalle.CantidadEntregada > 0
                || detalle.CantidadRechazada > 0;

            return hayActividad
                ? EstatusPedidoDetalleEnm.EN_PROCESO
                : EstatusPedidoDetalleEnm.PENDIENTE;
        }

        /// <summary>
        /// Determina el estatus final del header a partir del conjunto de líneas del pedido.
        /// Retorna null si el pedido aún no alcanza una condición terminal (todavía hay líneas no cerradas ni canceladas).
        /// Orden de reglas:
        /// 1. Si todas las líneas terminaron CANCELADO -> CANCELADO.
        /// 2. Si todas las líneas entregadas al 100% -> ENTREGADO.
        /// 3. Si ninguna línea entregó nada (entregada = 0 en todas) -> RECHAZADO.
        /// 4. En cualquier otro caso mixto -> ENTREGADO_PARCIAL.
        /// </summary>
        public static EstatusPedidoEnm? DeterminarEstatusFinalHeader(IEnumerable<OpPedidoDetalleEtt> lineas)
        {
            var lista = lineas as IReadOnlyCollection<OpPedidoDetalleEtt> ?? lineas.ToList();
            if (lista.Count == 0) return null;

            bool todasLineasCerradas = lista.All(d =>
                d.IdCtEstatusPedidoDetalle == (byte)EstatusPedidoDetalleEnm.CERRADO ||
                d.IdCtEstatusPedidoDetalle == (byte)EstatusPedidoDetalleEnm.CANCELADO);

            if (!todasLineasCerradas)
            {
                return null;
            }

            // Regla 1: Si todas las líneas son CANCELADO
            if (lista.All(d => d.IdCtEstatusPedidoDetalle == (byte)EstatusPedidoDetalleEnm.CANCELADO || d.CantidadCancelada >= d.Cantidad))
            {
                return EstatusPedidoEnm.CANCELADO;
            }

            // Regla 2: Entregado total
            var entregadoTotal = lista.All(d => d.CantidadEntregada == d.Cantidad);
            if (entregadoTotal)
            {
                return EstatusPedidoEnm.ENTREGADO;
            }

            // Regla 3: Rechazo total (nada fue entregado)
            var rechazoTotal = lista.All(d => d.CantidadEntregada == 0);
            if (rechazoTotal)
            {
                return EstatusPedidoEnm.RECHAZADO;
            }

            // Regla 4: Entregado parcial
            return EstatusPedidoEnm.ENTREGADO_PARCIAL;
        }
    }
}
