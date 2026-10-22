using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Queries
{
    /// <summary>
    /// Encapsula las reglas de negocio previas a la creación de un pedido:
    /// Regla Cliente-1: si es Mostrador, solo canal PRESENCIAL.
    /// Regla Cliente-2 y 10: cliente registrado no puede tener más de un pedido
    /// con TotalPagado menor a Total en la misma sucursal.
    /// Regla Cliente-3 y 9: si es Mostrador, PagoFld debe ser exactamente el Total.
    /// Regla Cliente-4 (Apartado): si EsApartado, el cliente NO puede ser Mostrador.
    /// Regla Vendedor-3: EntregaMostrador solo si ctProd.EntregaMostrador es true.
    /// Regla Vendedor-4: BajoPedido sin stock no puede marcarse EntregadoMostrador.
    /// Regla Vendedor-5: si EsApartado, no se permite marcar EntregaMostrador.
    /// Devuelve un OperationResult bool sin mensaje de éxito; los .Info/.Error
    /// contienen el motivo de rechazo en su Message.
    /// </summary>
    public record ValidarCapturaPedidoQry(
        byte IdSucursal,
        int IdCliente,
        byte IdCanalCaptura,
        bool EsMostrador,
        decimal Total,
        decimal PagoFld,
        bool EsApartado,
        List<ValidarCapturaPedidoProducto> Productos
    ) : IRequest<OperationResult<bool>>;

        /// <summary>
    /// Representa la registro (record) para validar captura pedido producto.
    /// </summary>
public record ValidarCapturaPedidoProducto(
        int IdProducto,
        bool ProductoEntregaMostrador,
        bool ProductoBajoPedido,
        bool ProductoMarcadoEntregadoMostrador,
        decimal ExistenciaDisponibleUBI
    );
}