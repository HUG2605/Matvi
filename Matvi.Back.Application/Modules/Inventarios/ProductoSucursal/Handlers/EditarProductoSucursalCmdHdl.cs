using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Handlers
{
    [Transaction]
    class EditarProductoSucursalCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<EditarProductoSucursalCmd, OperationResult<bool>>
    {
        public async Task<OperationResult<bool>> Handle(
            EditarProductoSucursalCmd request, 
            CancellationToken cancellationToken)
        {
            var prodSucuDto = request.productoSucursal;

            var prodSucuEtt = _unitOfWork.Query<RlProductoSucursalEtt>()
                .FirstOrDefault(r => r.IdCtProducto == prodSucuDto.CtProd_Id
                                  && r.IdCtSucursal == prodSucuDto.CtSuc_Id);
            if (prodSucuEtt == null)
                return OperationResult<bool>.Error("No se encontró la relación producto-sucursal.");

            prodSucuEtt.StockMinimoUBI = prodSucuDto.StockMinimoUBIFld;
            prodSucuEtt.StockMaximoUBI = prodSucuDto.StockMaximoUBIFld;
            prodSucuEtt.Ubicacion = prodSucuDto.UbicacionFld;
            _unitOfWork.Update(prodSucuEtt);

            return OperationResult<bool>.Success(true, "El producto en la sucursal se actualizó correctamente.");
        }
    }
}