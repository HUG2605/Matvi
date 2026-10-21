using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Queries
{
    public record ListarPedidoQry(
        byte idSucursal, 
        byte idRol,
        short? idUsuario,
        bool puedeVerSensibles,
        FilterGrd filter
        ): IRequest<DataGrd<ListarPedidoDto>>;
}