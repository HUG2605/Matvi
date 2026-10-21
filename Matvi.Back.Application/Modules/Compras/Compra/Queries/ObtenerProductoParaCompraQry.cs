using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Compras.Compra.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Compras.Compra.Queries
{
    public record ObtenerProductoParaCompraQry(
        int idPresentacionProducto,
        decimal cantidad,
        decimal costoUP,
        byte idSucursal = 0
    ) : IRequest<OperationResult<AgregarProductoCompraDto?>>;
}