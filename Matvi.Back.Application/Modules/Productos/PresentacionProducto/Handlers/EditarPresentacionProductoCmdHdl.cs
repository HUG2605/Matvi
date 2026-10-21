using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Handlers
{
    [Transaction]
    class EditarPresentacionProductoCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<EditarPresentacionProductoCmd, OperationResult<bool>>
    {
        public async Task<OperationResult<bool>> Handle(
            EditarPresentacionProductoCmd request, 
            CancellationToken cancellationToken)
        {
            var presProdDto = request.presentacionProducto;

            #region VALIDACIONES

            #region PRECIO DE MAYOREO

            // Validación: si uno tiene valor, el otro también debe tener
            bool tienePrecioMayoreo = presProdDto.PrecioMayoreoFld.HasValue;
            bool tieneUnidadesMayoreo = presProdDto.UnidadesMayoreoFld.HasValue;

            if (tienePrecioMayoreo && !tieneUnidadesMayoreo)
            {
                return OperationResult<bool>.Info(
                    false,
                    "Debe especificar la cantidad mínima para mayoreo cuando se define un precio de mayoreo.");
            }

            if (!tienePrecioMayoreo && tieneUnidadesMayoreo)
            {
                return OperationResult<bool>.Info(
                    false,
                    "Debe especificar el precio de mayoreo cuando se define una cantidad mínima para mayoreo.");
            }

            if (tienePrecioMayoreo && presProdDto.PrecioMayoreoFld >= presProdDto.PrecioVentaFld)
            {
                return OperationResult<bool>.Info(
                    false,
                    "El precio de mayoreo debe ser menor al precio de venta.");
            }

            #endregion

            #endregion

            #region ACTUALIZAR NUEVA PRESENTACION DEL PRODUCTO

            var presProdEtt = await _unitOfWork.FindAsync<CtPresentacionProductoEtt, int>(presProdDto.Id, cancellationToken);

            var ubiNombre = await _unitOfWork.Query<CtPresentacionProductoEtt>()
                .Where(pp => pp.Id == presProdDto.Id)
                .Select(pp => pp.Nav_IdCtProducto.Nav_IdCtUnidadBaseInventario.Nombre)
                .SingleOrDefaultAsync();

            // El Nombre y el FactorUBI solo se modifican si la presentacion
            // del producto no corresponde a la unidad base de inventarios.
            if (presProdEtt!.Nombre != ubiNombre)
            {
                presProdEtt.Nombre = presProdDto.NombreFld;
                presProdEtt.FactorUBI = presProdDto.FactorUBIFld;
            }
            presProdEtt.EsParaCompra = presProdDto.EsParaCompraFld;
            presProdEtt.EsParaVenta = presProdDto.EsParaVentaFld;
            presProdEtt.PrecioVenta = presProdDto.PrecioVentaFld;
            presProdEtt.PrecioMayoreo = presProdDto.PrecioMayoreoFld;
            presProdEtt.UnidadesMayoreo = presProdDto.UnidadesMayoreoFld;

            _unitOfWork.Update(presProdEtt);

            #endregion

            return OperationResult<bool>.Success(true, 
                "El producto se editó correctamente.");
        }
    }
}
