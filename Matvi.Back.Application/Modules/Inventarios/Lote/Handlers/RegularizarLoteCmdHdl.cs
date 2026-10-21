using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Inventarios.Lote.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Inventarios.Lote.Handlers
{
    [Transaction]
    class RegularizarLoteCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<RegularizarLoteCmd, OperationResult<bool>>
    {
        public async Task<OperationResult<bool>> Handle(
            RegularizarLoteCmd request,
            CancellationToken cancellationToken)
        {
            // 1. Buscar el lote
            var loteEtt = await _unitOfWork.FindAsync<OpLoteEtt, int>(request.Lote.Id, cancellationToken);
            if (loteEtt == null)
                return OperationResult<bool>.Error("Lote no encontrado.");

            // 2. Validar que sea de tipo INICIAL
            if (loteEtt.IdCtTipoLote != (byte)TipoLoteEnm.INICIAL)
                return OperationResult<bool>.Error("Solo se pueden regularizar lotes de tipo INICIAL.");

            // 3. Calcular cantidades
            // CantidadDisponibleUBI puede estar negativa por ventas previas a la regularización
            decimal vendidoUBI = Math.Abs(Math.Min(loteEtt.CantidadDisponibleUBI, 0));
            loteEtt.CantidadInicialUBI = vendidoUBI + request.Lote.ExistenciaActualFld;
            loteEtt.CantidadDisponibleUBI = request.Lote.ExistenciaActualFld;

            // 4. Asignar costos
            loteEtt.CostoUP = request.Lote.CostoUnitarioFld;
            loteEtt.CostoUBI = request.Lote.CostoUnitarioFld / loteEtt.Cpp_FactorUBI;

            _unitOfWork.Update(loteEtt);

            // 5. Marcar el producto como regularizado en la sucursal
            var relEtt = _unitOfWork.Query<RlProductoSucursalEtt>()
                .FirstOrDefault(r => 
                    r.IdCtProducto == loteEtt.Cpp_IdCtProducto
                 && r.IdCtSucursal == loteEtt.IdCtSucursal);
        
            if (relEtt == null)
                return OperationResult<bool>.Error("No se encontró la relación producto-sucursal.");

            relEtt.Regularizado = true;
            _unitOfWork.Update(relEtt);

            return OperationResult<bool>.Success(
                true, 
                "El lote se regularizó correctamente.");
        }
    }
}
