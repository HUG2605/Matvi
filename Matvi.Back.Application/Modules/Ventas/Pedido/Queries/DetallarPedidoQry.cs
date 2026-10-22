using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de detallar pedido.
    /// </summary>
public record DetallarPedidoQry(int Id, bool puedeVerSensibles) :
        IRequest<DetallarPedidoDto>;
}